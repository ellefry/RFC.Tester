using BHSW2_2.Pinion.DataService.AppServices.Interfaces;
using BHSW2_2.Pinion.DataService.Clients.Abstracts;
using BHSW2_2.Pinion.DataService.Clients.Dtos;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace BHSW2_2.Pinion.DataService.AppServices
{
    public class SapRequestAppService : ISapRequestAppService
    {
        private readonly SapConnectorContext _sapConnectorContext;

        public SapRequestAppService(SapConnectorContext sapConnectorContext)
        {
            _sapConnectorContext = sapConnectorContext;
        }

        public async Task FinishPartAsync(FinishPartInput input)
        {
            var sapRequest = new SapRequest
            {
                Id = Guid.NewGuid(),
                Content = JsonConvert.SerializeObject(input),
                FunctionName = "FinishPartSapServiceHandler",
            };
            _sapConnectorContext.SapRequests.Add(sapRequest);
            await _sapConnectorContext.SaveChangesAsync();
        }
    }
}
