using SapWebService.Common.Abstracts;
using System.Threading.Tasks;

namespace SapWebService.Common
{
    public class OutboundService : IOutboundService
    {
        public async Task<si_wms0007_outboundResponse> Transfer(dt_wms0007_reqStockTransferc transfer, 
            string address, string username, string password)
        {
            si_wms0007_outboundClient client =
                new si_wms0007_outboundClient(si_wms0007_outboundClient.EndpointConfiguration.HTTP_Port, address);

            client.ClientCredentials.UserName.UserName = username;
            client.ClientCredentials.UserName.Password = password;

            var ret = await client.si_wms0007_outboundAsync(
                new dt_wms0007_reqStockTransferc[] { new dt_wms0007_reqStockTransferc() });
            return ret;
        }
    }
}
