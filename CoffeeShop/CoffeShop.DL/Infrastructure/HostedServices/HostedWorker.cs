using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoffeShop.DL.Infrastructure.HostedServices
{
    internal class HostedWorker : IHostedService
    {
        private readonly ILogger<HostedWorker> _logger;

        public HostedWorker(ILogger<HostedWorker> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    _logger.LogInformation($"Test HostedWorker: {DateTime.UtcNow}");
                    await Task.Delay(1000, cancellationToken);
                }
            }, cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
