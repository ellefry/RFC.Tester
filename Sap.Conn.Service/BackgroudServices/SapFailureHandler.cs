using Sap.Conn.Service.DataStorage;
using Sap.Conn.Service.Domains;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;

namespace Sap.Conn.Service.BackgroudServices
{
    public class SapFailureHandler
    {
        private static readonly SemaphoreSlim ConnectionLock = new SemaphoreSlim(initialCount: 1, maxCount: 1);

        public void ProcessFailure()
        {
            ConnectionLock.Wait();
            try
            {
                using (var db = new SapConnectorContext())
                {
                    db.Set<ProcessRequest>().Add(new ProcessRequest { Id = Guid.NewGuid(), FunctionType = 0 });
                    db.SaveChanges();
                }
                Debug.Write($"{DateTime.Now} {Environment.NewLine}");
            }
            finally
            {
                ConnectionLock.Release();
            }

            
        }
    }
}