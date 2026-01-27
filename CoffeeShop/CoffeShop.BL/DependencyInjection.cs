using CoffeShop.BL.Interfaces;
using CoffeShop.BL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeShop.BL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLayer(this IServiceCollection services)
        {
            services.AddSingleton<ICoffeeCrudService, CoffeeCrudService>();
            services.AddSingleton<ISellCoffee, SellCoffee>();
            services.AddSingleton<ICustomerCrudService, CustomerCrudService>();

            return services;
        }
    }
}
