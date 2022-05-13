using BHSW2_2.Pinion.DataService.AppServices.Dtos;
using BHSW2_2.Pinion.DataService.AppServices.Interfaces;
using BHSW2_2.Pinion.DataService.Clients.Abstracts;
using BHSW2_2.Pinion.DataService.Clients.Dtos;
using BHSW2_2.Pinion.DataService.FailureHandlers;
using BHSW2_2.Pinion.DataService.FailureHandlers.Interfaces;
using BHSW2_2.Pinion.DataService.Interfaces;
using Microsoft.EntityFrameworkCore;
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
        private readonly ISapSwitcher _sapSwitcher;
        private readonly IEnumerable<ISapServiceHandler> _sapServiceHandlers;

        public SapRequestAppService(SapConnectorContext sapConnectorContext,
            IEnumerable<ISapServiceHandler> sapServiceHandlers, ISapSwitcher sapSwitcher)
        {
            _sapConnectorContext = sapConnectorContext;
            _sapServiceHandlers = sapServiceHandlers;
            _sapSwitcher = sapSwitcher;
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

        public async Task ScrapPartAsync(ScrapPartInput input)
        {
            var sapRequest = new SapRequest
            {
                Id = Guid.NewGuid(),
                Content = JsonConvert.SerializeObject(input),
                FunctionName = "ScrapPartSapServiceHandler",
            };
            _sapConnectorContext.SapRequests.Add(sapRequest);
            await _sapConnectorContext.SaveChangesAsync();
        }

        public async Task<List<SapRequest>> GetSapRequests(GetSapRequestsInput input)
        {
            var query = _sapConnectorContext.SapRequests.AsQueryable();
            if (!string.IsNullOrWhiteSpace(input?.SapRequestType))
                query = query.Where(q => q.FunctionName.StartsWith(input.SapRequestType));
            var itemsCount = (input?.ItemCount) ?? 100;
            query = query.OrderByDescending(q => q.Retries).Take(itemsCount);
            return await query.ToListAsync();
        }

        public async Task ReSendSapRequest(ReSendSapRequestInput input)
        {
            var sapRequest = await _sapConnectorContext.SapRequests.FirstOrDefaultAsync(s => s.Id == input.SapRequestId);
            if (sapRequest == null)
                throw new ApplicationException($"SapRequest doen not existed: [{input.SapRequestId}]");
            sapRequest.UpateProcessOrder(input.Content, input.ProcessOrder);
            _sapConnectorContext.Entry(sapRequest).State = EntityState.Modified;
            await _sapConnectorContext.SaveChangesAsync();
        }

        public async Task ProcessSapRequest()
        {
            var sapRequests = _sapConnectorContext.SapRequests.OrderByDescending(s => s.ProcessOrder).ToList();
            var switcherEnabled = await _sapSwitcher.GetSwitcherStatus();
            foreach (var sapRequest in sapRequests)
            {
                if (!switcherEnabled)
                    return;

                var handler = _sapServiceHandlers.FirstOrDefault(h => h.GetType().Name == sapRequest.FunctionName);
                await handler?.Handle(sapRequest);
            }
        }

        public async Task<List<SapRequestHistory>> GetSapHistories()
        {
            var query = _sapConnectorContext.SapRequestHistories.OrderByDescending(h => h.Created).Take(100);
            return await query.ToListAsync();
        }

        public async Task OutboundTransfer(OutboundTransferInput input)
        {
            var sapRequest = new SapRequest
            {
                Id = Guid.NewGuid(),
                Content = JsonConvert.SerializeObject(input),
                FunctionName = nameof(OutboundTransferServiceHandler),
            };
            _sapConnectorContext.SapRequests.Add(sapRequest);
            await _sapConnectorContext.SaveChangesAsync();
        }
    }
}
