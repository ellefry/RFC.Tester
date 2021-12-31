using BHSW2_2.Pinion.DataService.Clients.Dtos;
using System.Threading.Tasks;

namespace BHSW2_2.Pinion.DataService.Clients.Abstracts
{
    public interface ISapConnectorClient
    {
        Task<string> FinishPart(FinishPartInput input);
        Task<string> ScrapPart(ScrapPartInput input);
    }
}
