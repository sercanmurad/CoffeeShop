using CoffeShop.DL.Infrastructure.HostedServices;
using CoffeShop.DL.Interfaces;
using CoffeShop.DL.Kafka;
using CoffeShop.DL.Repositories;
using CoffeShop.Models.Configurations;
using CoffeShop.Models.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace CoffeShop.DL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataLayer(this IServiceCollection services, IConfiguration configs)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

            services
                .AddHostedService<BackgroundWorker>()
                .AddHostedService<HostedWorker>()
                .AddConfigurations(configs)
                .AddSingleton<ICoffeeRepository, CoffeeLocalRepository>()
                .AddSingleton<ICoffeeRepository, CoffeeMongoRepository>()
                .AddSingleton<ICustomerRepository, CustomerMongoRepository>();

            return services;
        }

        private static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configs)
        {
            services.Configure<MongoDbConfiguration>(configs.GetSection(nameof(MongoDbConfiguration)));

            var kafkaSettings = configs.GetSection(nameof(KafkaSettings)).Get<KafkaSettings>();
            services.AddSingleton(kafkaSettings!);

            services.AddSingleton(sp => new GenericKafkaProducer<string, SellCoffeeResult>(kafkaSettings!));

            services.AddHostedService<KafkaConsumerWorker>();

            return services;
        }
    }
}
