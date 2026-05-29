using System.Threading;
using System.Threading.Tasks;
using CoffeShop.DL.Kafka;
using CoffeShop.Models.Responses;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoffeShop.DL.Infrastructure.HostedServices
{
    internal class BackgroundWorker : BackgroundService
    {
        private readonly ILogger<BackgroundWorker> _logger;
        private readonly KafkaSettings _kafkaSettings;

        public BackgroundWorker(ILogger<BackgroundWorker> logger, KafkaSettings kafkaSettings)
        {
            _logger = logger;
            _kafkaSettings = kafkaSettings;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() =>
            {
                var consumer = new GenericKafkaConsumer<string, SellCoffeeResult>(_kafkaSettings, (key, value) =>
                {
                    _logger.LogInformation(
                        $"[KAFKA] Coffee sold! Coffee: {value.Coffee.Name}, Customer: {value.Customer.Name}, Price: {value.Price}");
                });

                consumer.StartConsuming(stoppingToken);
            }, stoppingToken);
        }
    }
}
