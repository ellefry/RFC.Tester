using RFC.Common;
using RFC.Common.Interfaces;
using Sap.Conn.Service.DataStorage;
using Sap.Conn.Service.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Sap.Conn.Service.Controllers
{
    public class SapController : Controller
    {
        private readonly IRfcManager _rfcmanager;

        public SapController(IRfcManager rfcmanager)
        {
            _rfcmanager = rfcmanager;
        }

        [HttpGet]
        public async Task Failures()
        {
            //_rfcmanager.ProcessRequest(new ProcessRequestInput());
            await Task.CompletedTask;
        }

        [HttpPost]
        public async Task ProcessRequest(ProcessRequestInput input)
        {
            try
            {
                _rfcmanager.ProcessRequest(input);
                await Task.CompletedTask;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}