using BHSW2_2.Pinion.DataService.Clients.Abstracts;
using BHSW2_2.Pinion.DataService.Clients.Dtos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace BHSW2_2.Pinion.DataService.FailureHandlers
{
    public class FinishPartSapServiceHandler : SapServiceHandlerBase
    {
        private readonly ISapConnectorClient _sapConnectorClient;

        public FinishPartSapServiceHandler(SapConnectorContext dbContext, ISapConnectorClient sapConnectorClient) 
            : base(dbContext)
        {
            _sapConnectorClient = sapConnectorClient;
        }

        protected override async Task<string> ProcessSapRequest(SapRequest sapRequest)
        {
            var input = JsonConvert.DeserializeObject<FinishPartInput>(sapRequest.Content);
            return await _sapConnectorClient.FinishPart(input);
        }
    }
}
