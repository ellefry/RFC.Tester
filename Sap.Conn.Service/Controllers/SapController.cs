using RFC.Common;
using RFC.Common.Interfaces;
using Sap.Conn.Service.AppServices.Interfaces;
using Sap.Conn.Service.Domains.Interfaces;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sap.Conn.Service.Controllers
{
    public class SapController : ApiController
    {
        private readonly IRfcManager _rfcmanager;

        public SapController(IRfcManager rfcmanager)
        {
            _rfcmanager = rfcmanager;
        }

        [HttpPost]
        public async Task<ProcessRequestInput> ProcessSapRequest(ProcessRequestInput input)
        {
             return await Task.FromResult(_rfcmanager.ProcessRequest(input));
        }
     
    }
}