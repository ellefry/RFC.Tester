using Autofac;
using Hangfire;
using Sap.Conn.Service.App_Start;
using Sap.Conn.Service.AppServices.Interfaces;
using System.Threading;

namespace Sap.Conn.Service.BackgroudServices
{
    public class SapFailureHandler
    {
        private static readonly SemaphoreSlim ConnectionLock = new SemaphoreSlim(initialCount: 1, maxCount: 1);

        [AutomaticRetry(Attempts = 0, LogEvents = false, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public void ProcessFailure()
        {
            ConnectionLock.Wait();
            try
            {
                using (var scope = IocContainerBuilder.IocContainer.BeginLifetimeScope())
                {
                    // Resolve services from a scope that is a child of the root container.
                    var service = scope.Resolve<IProcessRequestAppService>();
                    service.ProcessFailedSapRfcRequest();

                }
            }
            finally
            {
                ConnectionLock.Release();
            }
        }
    }
}