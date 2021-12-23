using BHSW2_2.Pinion.DataService.Clients.Abstracts;
using BHSW2_2.Pinion.DataService.Clients.Dtos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace BHSW2_2.Pinion.DataService.FailureHandlers
{
    public class ScrapPartSapServiceHandler : SapServiceHandlerBase
    {
        private readonly ISapConnectorClient _sapConnectorClient;

        public ScrapPartSapServiceHandler(SapConnectorContext dbContext, ISapConnectorClient sapConnectorClient) 
            : base(dbContext)
        {
            _sapConnectorClient = sapConnectorClient;
        }

        protected override async Task ProcessSapRequest(SapRequest sapRequest)
        {
            var input = JsonConvert.DeserializeObject<ScrapPartInput>(sapRequest.Content);
            await _sapConnectorClient.ScrapPart(input);
        }
    }
}
