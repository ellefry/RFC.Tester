using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
        public async Task<MesApiMetaData> Get()
        {
            return new MesApiMetaData
            {
                ApiVersion = "1.0.0",
                Name = _configuration.GetValue<string>("ServiceName") ?? "BHSW2_2.Pinion.Api",
                AssemblyVersion = this.GetType().Assembly.GetName().Version.ToString()
            };
        }
    }


    public class MesApiMetaData
    {
        /// <summary>
        /// ApiVersion
        /// </summary>
        public string ApiVersion { get; set; }


        /// <summary>
        /// AssemblyVersion
        /// </summary>
        public string AssemblyVersion { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
    }
}
