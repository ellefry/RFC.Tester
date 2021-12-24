using BHSW2_2.Pinion.DataService.AppServices.Interfaces;
using BHSW2_2.Pinion.DataService.Clients.Dtos;
using BHSW2_2.Pinion.DataService.Extensions;
using BHSW2_2.Pinion.DataService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BHSW2_2.Pinion.DataService.Controllers
{
    [ApiController]
    [Route("api/sap")]
    public class SapController : ControllerBase
    {
        private readonly ISapRequestAppService _sapRequestAppService;
        private readonly ISapSwitcher _sapSwitcher;

        public SapController(ISapRequestAppService sapRequestAppService, ISapSwitcher sapSwitcher)
        {
            _sapRequestAppService = sapRequestAppService;
            _sapSwitcher = sapSwitcher;
        }

        [HttpPost("FinishPart")]
        public async Task<ResponseResult> FinishPart(FinishPartInput input)
        {
            await _sapRequestAppService.FinishPartAsync(input);
            return new ResponseResult();
        }

        [HttpPost("ScrapPart")]
        public async Task<ResponseResult> ScrapPart(ScrapPartInput input)
        {
            await _sapRequestAppService.ScrapPartAsync(input);
            return new ResponseResult();
        }

        [HttpGet("switcher")]
        public async Task<bool> GetSapSwitcher()
        {
            return await Task.FromResult(_sapSwitcher.IsEnabled);
        }

        [HttpPost("switcher/{enabled}")]
        public async Task GetSapSwitcher(bool enabled)
        {
            _sapSwitcher.IsEnabled = enabled;
            await Task.CompletedTask;
        }
    }
}
