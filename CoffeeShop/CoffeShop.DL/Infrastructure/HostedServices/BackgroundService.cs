using System.Threading;
using System.Threading.Tasks;
using CoffeShop.DL.Kafka;
using CoffeShop.Models.Responses;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoffeShop.DL.Infrastructure.HostedServices
{
    public class KafkaConsumerWorker : BackgroundService
    {
        private readonly ILogger<KafkaConsumerWorker> _logger;
        private readonly KafkaSettings _kafkaSettings;

        public KafkaConsumerWorker(ILogger<KafkaConsumerWorker> logger, KafkaSettings kafkaSettings)
        {
            _logger = logger;
            _kafkaSettings = kafkaSettings;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Kafka Consumer Worker is starting in the background.");

            return Task.Run(() => RunConsumerLoop(stoppingToken), stoppingToken);
        }

        private void RunConsumerLoop(CancellationToken stoppingToken)
        {
            var consumer = new GenericKafkaConsumer<string, SellCoffeeResult>(
                _kafkaSettings,
                onMessageReceived: (key, receivedModel) =>
                {
                    string coffeeInfo = receivedModel.Coffee != null
                        ? $"{receivedModel.Coffee.RoastYear} {receivedModel.Coffee.Name}"
                        : "Unknown Coffee";

                    string customerName = receivedModel.Customer != null
                        ? receivedModel.Customer.Name
                        : "Unknown Customer";

                    _logger.LogInformation(
                        "\n====== NEW COFFEE SALE RECEIVED FROM KAFKA ======\n" +
                        "Kafka Key: {Key}\n" +
                        "Customer:  {CustomerName}\n" +
                        "Coffee:    {CoffeeInfo}\n" +
                        "Sale Price:${Price}\n" +
                        "==============================================",
                        key, customerName, coffeeInfo, receivedModel.Price);
                });

            consumer.StartConsuming(stoppingToken);

            _logger.LogInformation("Kafka Consumer Worker has cleanly stopped.");
        }
    }
}
