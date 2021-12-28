using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using RFC.Common;
using RFC.Common.Interfaces;
using Sap.Conn.Service.AppServices;
using Sap.Conn.Service.AppServices.Interfaces;
using Sap.Conn.Service.DataStorage;
using Sap.Conn.Service.Domains;
using Sap.Conn.Service.Domains.Interfaces;
using System.Configuration;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;

namespace Sap.Conn.Service.App_Start
{
    public class IocContainerBuilder
    {
        public static IContainer IocContainer;

        public static void Build()
        {
            var builder = new ContainerBuilder();

            // Register your MVC controllers. (MvcApplication is the name of
            // the class in Global.asax.)
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // OPTIONAL: Register model binders that require DI.
            builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            builder.RegisterModelBinderProvider();

            // OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            // OPTIONAL: Enable property injection in view pages.
            builder.RegisterSource(new ViewRegistrationSource());

            // OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();

            builder.RegisterType<SapConnectorContext>().InstancePerLifetimeScope();
            var sapConfig = CreateSapConfiguration();
            builder.RegisterInstance<SapConnectionConfig>(sapConfig).SingleInstance();
            builder.RegisterType<RfcManager>().As<IRfcManager>().InstancePerLifetimeScope();
            builder.RegisterType<RfcRepositoryCreator>().As<IRfcRepositoryCreator>().InstancePerLifetimeScope();
            builder.RegisterType<RfcFunctionOperator>().As<IRfcFunctionOperator>().InstancePerLifetimeScope();

            //Application Services
            builder.RegisterType<ProcessRequestAppService>().As<IProcessRequestAppService>().InstancePerLifetimeScope();

            builder.RegisterType<SapSwitcher>().As<ISapSwitcher>().InstancePerLifetimeScope();

            // Set the dependency resolver to be Autofac.
            IocContainer = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(IocContainer);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(IocContainer));
        }

        static SapConnectionConfig CreateSapConfiguration()
        {
            var config = new SapConnectionConfig();
            config.IPAddress = ConfigurationManager.AppSettings["sap:IPAddress"];
            config.SystemNumber = ConfigurationManager.AppSettings["sap:SystemNumber"];
            config.SystemID = ConfigurationManager.AppSettings["sap:SystemID"];
            config.User = ConfigurationManager.AppSettings["sap:User"];
            config.Password = ConfigurationManager.AppSettings["sap:Password"];
            config.Client = ConfigurationManager.AppSettings["sap:Client"];
            config.Language = ConfigurationManager.AppSettings["sap:Language"];
            config.PoolSize = "5";
            config.ReposotoryPassword = "";
            return config;
        }
    }
}