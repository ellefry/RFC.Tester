using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BHSW2_2.Pinion.DataService.Controllers
{
    [ApiController]
    [Route("api/sap")]
    public class SapController : ControllerBase
    {
        public SapController()
        {
        }

        [HttpPost("FinishPart")]
        public async Task FinishPart()
        {
            await Task.CompletedTask;
        }
    }
}
