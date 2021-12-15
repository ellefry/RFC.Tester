using RFC.Common;
using Sap.Conn.Service.AppServices.Interfaces;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sap.Conn.Service.Controllers
{
    public class SapController : ApiController
    {
        private readonly IProcessRequestAppService _processRequestAppService;

        public SapController(IProcessRequestAppService processRequestAppService)
        {
            _processRequestAppService = processRequestAppService;
        }

        [HttpGet]
        public async Task Failures()
        {
            await Task.CompletedTask;
        }

        [HttpPost]
        public async Task ProcessSapRequest(ProcessRequestInput input)
        {
           await _processRequestAppService.ProcessSapRfcRequest(input);
        }
    }
}