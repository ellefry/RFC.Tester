using RFC.Common;
using Sap.Conn.Service.AppServices.Interfaces;
using Sap.Conn.Service.Domains.Interfaces;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sap.Conn.Service.Controllers
{
    public class SapController : ApiController
    {
        private readonly IProcessRequestAppService _processRequestAppService;
        private readonly ISapSwitcher _sapSwitcher;

        public SapController(IProcessRequestAppService processRequestAppService, ISapSwitcher sapSwitcher)
        {
            _processRequestAppService = processRequestAppService;
            _sapSwitcher = sapSwitcher;
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

        [HttpGet]
        [Route("switcher")]
        public async Task<bool> GetSapSwitcher()
        {
            return await Task.FromResult(_sapSwitcher.IsEnabled);
        }

        [HttpPost]
        [Route("switcher/{enabled}")]
        public async Task GetSapSwitcher(bool enabled)
        {
            _sapSwitcher.IsEnabled = enabled;
            await Task.CompletedTask;
        }
    }
}