using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using BHSW2_2.Pinion.DataService.AppServices.Interfaces;

namespace BHSW2_2.Pinion.DataService.BackgroudServices
{
    public class SapRequestBackgroudService : BackgroundService
    {
        private readonly ILogger<SapRequestBackgroudService> logger;
        private readonly IServiceProvider _serviceProvider;

        public SapRequestBackgroudService(ILogger<SapRequestBackgroudService> logger, IServiceProvider serviceProvider)
        {
            this.logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    logger.LogTrace("Start retry send notifications >>>>>>>>>>>>");
                    await Handle();
                }
                catch (Exception e)
                {
                    logger.LogError(e, e.Message);
                }
                finally
                {
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }
        }

        private async Task Handle()
        {
            using var scope = _serviceProvider.CreateScope();
            var sapRequestAppService = scope.ServiceProvider.GetRequiredService<ISapRequestAppService>();
            await sapRequestAppService.ProcessSapRequest();
        }
    }
}
