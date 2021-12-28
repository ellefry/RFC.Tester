using BHSW2_2.Pinion.DataService.AppServices;
using BHSW2_2.Pinion.DataService.AppServices.Interfaces;
using BHSW2_2.Pinion.DataService.BackgroudServices;
using BHSW2_2.Pinion.DataService.Clients;
using BHSW2_2.Pinion.DataService.Clients.Abstracts;
using BHSW2_2.Pinion.DataService.Extensions;
using BHSW2_2.Pinion.DataService.FailureHandlers;
using BHSW2_2.Pinion.DataService.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BHSW2_2.Pinion.DataService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddHttpClient(SapConnectorConstants.SapConnectorName, c =>
            {
                c.BaseAddress = new Uri(Configuration.GetValue<string>(SapConnectorConstants.SapConnectorName));
            });

            //logic services
            services.AddScoped<ISapRequestAppService, SapRequestAppService>();
            services.AddScoped<ISapConnectorClient, SapConnectorClient>();
            services.AddFailureHandlerServices();
            services.AddHostedService<SapRequestBackgroudService>();
            services.AddSingleton<ISapSwitcher, SapSwitcher>();

            services.AddSwaggerGen();
            services.AddDbContext(Configuration);

            services.AddControllers()
                .AddMvcOptions(options => options.Filters.Add<HttpExceptionFilter>())
                .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy  = null);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.InitializeDatabase();
            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sap Connector V1");
                c.RoutePrefix = string.Empty;
                //c.DefaultModelsExpandDepth(-1);//hide model layer
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
