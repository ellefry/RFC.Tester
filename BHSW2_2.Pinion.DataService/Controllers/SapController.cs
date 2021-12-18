using BHSW2_2.Pinion.DataService.AppServices.Interfaces;
using BHSW2_2.Pinion.DataService.Clients.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BHSW2_2.Pinion.DataService.Controllers
{
    [ApiController]
    [Route("api/sap")]
    public class SapController : ControllerBase
    {
        private readonly ISapRequestAppService _sapRequestAppService;

        public SapController(ISapRequestAppService sapRequestAppService)
        {
            _sapRequestAppService = sapRequestAppService;
        }

        [HttpPost("FinishPart")]
        public async Task FinishPart(FinishPartInput input)
        {
            await _sapRequestAppService.FinishPartAsync(input);
        }
    }
}
