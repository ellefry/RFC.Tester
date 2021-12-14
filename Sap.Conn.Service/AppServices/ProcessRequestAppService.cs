using RFC.Common.Interfaces;
using Sap.Conn.Service.AppServices.Interfaces;
using Sap.Conn.Service.DataStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sap.Conn.Service.AppServices
{
    public class ProcessRequestAppService : IProcessRequestAppService
    {
        private readonly SapConnectorContext _dbContext;
        private readonly IRfcManager _rfcmanager;

        public ProcessRequestAppService(SapConnectorContext dbContext, IRfcManager rfcmanager)
        {
            _dbContext = dbContext;
            _rfcmanager = rfcmanager;
        }
    }
}