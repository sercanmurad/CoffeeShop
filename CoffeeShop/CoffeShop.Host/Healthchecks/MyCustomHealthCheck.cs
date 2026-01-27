using CoffeShop.Models.Configurations;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CoffeShop.Host.Healthchecks
{
    public class MyCustomHealthCheck : IHealthCheck
    {
        private readonly IOptionsMonitor<MongoDbConfiguration>? _mongoDbConfiguration;

        public MyCustomHealthCheck(IOptionsMonitor<MongoDbConfiguration>? mongoDbConfiguration)
        {
            _mongoDbConfiguration = mongoDbConfiguration;
        }

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            var isHealthy = false;

            try
            {
                var client = new MongoClient(_mongoDbConfiguration!.CurrentValue.ConnectionString);
                var database = client.GetDatabase(_mongoDbConfiguration.CurrentValue.DatabaseName);
                database.RunCommandAsync((Command<MongoDB.Bson.BsonDocument>)"{ping:1}").Wait(cancellationToken);
                isHealthy = true;
            }
            catch (Exception)
            {
                isHealthy = false;
            }

            if (isHealthy)
            {
                return Task.FromResult(HealthCheckResult.Healthy("MongoDB is healthy."));
            }

            return Task.FromResult(new HealthCheckResult(context.Registration.FailureStatus, "MongoDB is unhealthy."));
        }
    }
}
