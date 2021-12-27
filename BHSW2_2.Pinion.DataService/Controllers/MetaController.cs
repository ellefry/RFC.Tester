using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace BHSW2_2.Pinion.DataService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetaController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public MetaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<dynamic> Get()
        {
            var s = string.Empty;
            var s1 = string.Empty;
            return await Task.FromResult(
               new
               {
                   ApiVersion = "1.0.0",
                   Name = _configuration.GetValue<string>("ServiceName") ?? "BHSW2_2.Pinion.Api",
                   AssemblyVersion = this.GetType().Assembly.GetName().Version.ToString()
               });
        }
    }
}
