using CoffeShop.BL;
using CoffeShop.DL;
using CoffeShop.Host.Healthchecks;
using CoffeShop.Host.Validators;
using FluentValidation;
using Mapster;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace CoffeShop.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .CreateLogger();

            builder.Services
                .AddDataLayer(builder.Configuration)
                .AddBusinessLayer();

            builder.Services.AddMapster();

            builder.Services.AddValidatorsFromAssemblyContaining<AddCoffeeRequestValidator>();

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Coffe Shop", Version = "v1" });
            });

            builder.Host.UseSerilog();

            builder.Services
                .AddHealthChecks()
                .AddCheck<MyCustomHealthCheck>("sample");

            var app = builder.Build();

            app.MapHealthChecks("/healthz");

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("v1/swagger.json", "CoffeShop V1");
            });

            app.UseSwagger();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
