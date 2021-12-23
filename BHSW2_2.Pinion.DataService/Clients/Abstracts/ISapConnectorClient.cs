using BHSW2_2.Pinion.DataService.Clients.Dtos;
using System.Threading.Tasks;

namespace BHSW2_2.Pinion.DataService.Clients.Abstracts
{
    public interface ISapConnectorClient
    {
        Task FinishPart(FinishPartInput input);
        Task ScrapPart(ScrapPartInput input);
    }
}
