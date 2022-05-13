using BHSW2_2.Pinion.DataService.AppServices.Dtos;
using BHSW2_2.Pinion.DataService.Clients.Abstracts;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BHSW2_2.Pinion.DataService.FailureHandlers
{
    public class OutboundTransferServiceHandler : SapServiceHandlerBase
    {
        private readonly ISapConnectorClient _sapConnectorClient;

        public OutboundTransferServiceHandler(SapConnectorContext dbContext, ISapConnectorClient sapConnectorClient)
            : base(dbContext)
        {
            _sapConnectorClient = sapConnectorClient;
        }

        protected override async Task<string> ProcessSapRequest(SapRequest sapRequest)
        {
            var input = JsonConvert.DeserializeObject<OutboundTransferInput>(sapRequest.Content);
            return await _sapConnectorClient.Tranfer(input);
        }
    }
}
