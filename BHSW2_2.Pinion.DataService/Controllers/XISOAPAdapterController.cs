using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SapWebService.Common;
using System.Threading.Tasks;

namespace BHSW2_2.Pinion.DataService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class XISOAPAdapterController : ControllerBase
    {
        [HttpPost("MessageServlet")]
        public async Task<IActionResult> MessageServlet(dt_wms0007_reqStockTransferc[] transfers)
        {
            return Ok();
        }
    }
}
