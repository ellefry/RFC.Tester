using Hangfire.SqlServer;
using Hangfire;
using Sap.Conn.Service.DataStorage;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Diagnostics;
using Hangfire.Common;
using Sap.Conn.Service.BackgroudServices;
using Sap.Conn.Service.App_Start;

namespace Sap.Conn.Service
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            System.Web.Http.GlobalConfiguration.Configure(WebApiConfig.Register);
            IocContainerBuilder.Build();
            HangfireAspNet.Use(GetHangfireServers);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //RecurringJob.AddOrUpdate("powerfuljob", () => PowerfulJob(), "0/5 * * * * ?");
            RecurringJob.AddOrUpdate<SapFailureHandler>("SapFailureHandler", service => service.ProcessFailure(),
                "0/30 * * * * ?");
        }

        private IEnumerable<IDisposable> GetHangfireServers()
        {
            Hangfire.GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage("Server=.\\sql2019; Database=SapHangfire1; User ID=sa;Password=123456;MultipleActiveResultSets=True;Encrypt=False;TrustServerCertificate=False;",
                new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                });

            yield return new BackgroundJobServer();
        }
    }
}
