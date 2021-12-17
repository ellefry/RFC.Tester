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

        public override async Task Handle(SapRequest sapRequest)
        {
            try
            {
                var input = JsonConvert.DeserializeObject<FinishPartInput>(sapRequest.Content);
                await _sapConnectorClient.FinishPart(input);
                //remove to SapRequestHistory
                await MoveToHistory(sapRequest);
            }
            catch (Exception ex)
            {
                sapRequest.UpdateRetry(ex.Message);
                _dbContext.Entry(sapRequest).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
