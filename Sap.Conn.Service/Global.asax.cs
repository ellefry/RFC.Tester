using Hangfire;
using Hangfire.SqlServer;
using Sap.Conn.Service.App_Start;
using Sap.Conn.Service.BackgroudServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Sap.Conn.Service
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static bool SapOn { get; set; } = true;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            System.Web.Http.GlobalConfiguration.Configure(WebApiConfig.Register);
            IocContainerBuilder.Build();
            //HangfireAspNet.Use(GetHangfireServers);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            HttpException httpException = exception as HttpException;
            var httpContext = ((HttpApplication)sender).Context;
            httpContext.Response.StatusCode = 500;
            httpContext.Response.Clear();
            Server.ClearError();
        }

        private IEnumerable<IDisposable> GetHangfireServers()
        {
            var dbConnection = ConfigurationManager.AppSettings["HangfireDb"];
            Hangfire.GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(dbConnection,
                    new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true,

                    });

            yield return new BackgroundJobServer();
        }
    }
}
