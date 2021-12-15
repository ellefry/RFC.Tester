using BHSW2_2.Pinion.DataService.Clients.Dtos;
using System.Net.Http;
using System.Threading.Tasks;

namespace BHSW2_2.Pinion.DataService.Clients
{
    public class SapConnectorClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SapConnectorClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory=httpClientFactory;
        }

        public async Task FinishPart(FinishPartInput input)
        {

        }
    }
}
