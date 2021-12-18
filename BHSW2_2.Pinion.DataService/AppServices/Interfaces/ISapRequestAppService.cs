using BHSW2_2.Pinion.DataService.Clients.Dtos;
using System.Threading.Tasks;

namespace BHSW2_2.Pinion.DataService.AppServices.Interfaces
{
    public interface ISapRequestAppService
    {
        Task FinishPartAsync(FinishPartInput input);
        Task ProcessSapRequest();
    }
}
