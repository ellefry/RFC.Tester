using BHSW2_2.Pinion.DataService.FailureHandlers.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BHSW2_2.Pinion.DataService.FailureHandlers
{
    public static class FailureHanlderExtensions
    {
        public static void AddFailureHandlerServices(this IServiceCollection services)
        {
            services.AddScoped<ISapServiceHandler, FinishPartSapServiceHandler>();
            services.AddScoped<ISapServiceHandler, ScrapPartSapServiceHandler>();
        }
    }
}
