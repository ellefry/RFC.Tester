using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
