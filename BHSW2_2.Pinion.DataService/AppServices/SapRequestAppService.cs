using BHSW2_2.Pinion.DataService.AppServices.Interfaces;
using BHSW2_2.Pinion.DataService.Clients.Abstracts;
using BHSW2_2.Pinion.DataService.Clients.Dtos;
using BHSW2_2.Pinion.DataService.FailureHandlers.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BHSW2_2.Pinion.DataService.AppServices
{
    public class SapRequestAppService : ISapRequestAppService
    {
        private readonly SapConnectorContext _sapConnectorContext;
        private readonly IEnumerable<ISapServiceHandler> _sapServiceHandlers;

        public SapRequestAppService(SapConnectorContext sapConnectorContext, 
            IEnumerable<ISapServiceHandler> sapServiceHandlers)
        {
            _sapConnectorContext = sapConnectorContext;
            _sapServiceHandlers = sapServiceHandlers;
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

        public async Task ProcessSapRequest()
        {
            var sapRequests = _sapConnectorContext.SapRequests.ToList();
            foreach (var sapRequest in sapRequests)
            {
                var handler = _sapServiceHandlers.FirstOrDefault(h => h.GetType().Name == sapRequest.FunctionName);
                await handler?.Handle(sapRequest);
            }
        }
    }
}
