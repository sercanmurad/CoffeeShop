from __future__ import annotations

from dataclasses import dataclass, asdict
from math import floor
from typing import Any, Dict

from flask import Flask, jsonify, render_template, request


app = Flask(__name__)


@dataclass
class VendingInput:
    machine_price: float  # начална инвестиция
    drink_price: float  # продажна цена на напитка
    drink_cost: float  # себестойност на напитка (консумативи)
    drinks_per_day: float  # брой напитки на ден
    electricity_per_day: float  # разход за ток на ден
    monthly_fixed_costs: float  # фиксирани месечни разходи (части, сервиз и др.)


@dataclass
class VendingResult:
    daily_revenue: float
    daily_costs_variable: float
    daily_costs_electricity: float
    daily_profit: float
    monthly_profit: float
    yearly_profit: float
    payback_months: float | None
    payback_years: float | None
    payback_readable: str


def _safe_float(value: Any, default: float = 0.0) -> float:
    try:
        return float(value)
    except (TypeError, ValueError):
        return default


def calculate_vending_metrics(data: Dict[str, Any]) -> VendingResult:
    """Core calculation logic, reusable for tests or other front-ends."""
    inp = VendingInput(
        machine_price=_safe_float(data.get("machine_price")),
        drink_price=_safe_float(data.get("drink_price")),
        drink_cost=_safe_float(data.get("drink_cost")),
        drinks_per_day=_safe_float(data.get("drinks_per_day")),
        electricity_per_day=_safe_float(data.get("electricity_per_day")),
        monthly_fixed_costs=_safe_float(data.get("monthly_fixed_costs")),
    )

    # Basic sanity constraints
    if inp.machine_price < 0 or inp.drink_price < 0 or inp.drink_cost < 0:
        raise ValueError("Цените не могат да бъдат отрицателни.")
    if inp.drinks_per_day < 0 or inp.electricity_per_day < 0 or inp.monthly_fixed_costs < 0:
        raise ValueError("Броят напитки и разходите не могат да бъдат отрицателни.")

    daily_revenue = inp.drinks_per_day * inp.drink_price
    daily_costs_variable = inp.drinks_per_day * inp.drink_cost
    daily_costs_electricity = inp.electricity_per_day
    daily_profit = daily_revenue - daily_costs_variable - daily_costs_electricity

    # Приемаме ~30 дни месец за проста сметка
    monthly_profit = daily_profit * 30.0 - inp.monthly_fixed_costs
    yearly_profit = monthly_profit * 12.0

    if monthly_profit > 0:
        payback_months = inp.machine_price / monthly_profit if inp.machine_price > 0 else 0.0
        payback_years = payback_months / 12.0

        whole_months = floor(payback_months)
        remaining_days = floor((payback_months - whole_months) * 30)
        if whole_months <= 0 and remaining_days <= 0:
            payback_readable = "Възвръщаемостта е по-малка от месец."
        else:
            parts = []
            if whole_months > 0:
                parts.append(f"{whole_months} м.")
            if remaining_days > 0:
                parts.append(f"{remaining_days} дни")
            payback_readable = " + ".join(parts)
    else:
        payback_months = None
        payback_years = None
        payback_readable = "Инвестицията не се изплаща при тези параметри (месечната печалба е ≤ 0)."

    return VendingResult(
        daily_revenue=daily_revenue,
        daily_costs_variable=daily_costs_variable,
        daily_costs_electricity=daily_costs_electricity,
        daily_profit=daily_profit,
        monthly_profit=monthly_profit,
        yearly_profit=yearly_profit,
        payback_months=payback_months,
        payback_years=payback_years,
        payback_readable=payback_readable,
    )


@app.route("/", methods=["GET"])
def index() -> str:
    """Основна страница с формата и визуализацията."""
    return render_template("index.html")


@app.route("/api/calculate", methods=["POST"])
def api_calculate():
    """API крайна точка за калкулация – приема JSON и връща резултатите."""
    try:
        payload = request.get_json(force=True, silent=False) or {}
    except Exception:
        return jsonify({"error": "Невалидно JSON тяло."}), 400

    try:
        result = calculate_vending_metrics(payload)
    except ValueError as ex:
        return jsonify({"error": str(ex)}), 400

    return jsonify(asdict(result))


if __name__ == "__main__":
    # За локално стартиране по време на разработка
    app.run(host="0.0.0.0", port=8000, debug=True)

