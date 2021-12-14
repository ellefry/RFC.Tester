using Autofac;
using Autofac.Integration.Mvc;
using RFC.Common;
using RFC.Common.Interfaces;
using Sap.Conn.Service.DataStorage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sap.Conn.Service.App_Start
{
    public class IocContainerBuilder
    {
        public static void Build()
        {
            var builder = new ContainerBuilder();

            // Register your MVC controllers. (MvcApplication is the name of
            // the class in Global.asax.)
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

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

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
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
            return config;
        }
    }
}