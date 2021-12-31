using BHSW2_2.Pinion.DataService.AppServices.Dtos;
using BHSW2_2.Pinion.DataService.Clients.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHSW2_2.Pinion.DataService.AppServices.Interfaces
{
    public interface ISapRequestAppService
    {
        Task FinishPartAsync(FinishPartInput input);
        Task ScrapPartAsync(ScrapPartInput input);
        Task<List<SapRequest>> GetSapRequests(GetSapRequestsInput input);
        Task ReSendSapRequest(ReSendSapRequestInput input);
        Task ProcessSapRequest();
        Task<List<SapRequestHistory>> GetSapHistories();
    }
}
