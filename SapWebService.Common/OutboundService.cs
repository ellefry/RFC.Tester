using SapWebService.Common.Abstracts;
using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace SapWebService.Common
{
    public class OutboundService : IOutboundService
    {
        public si_wms0007_outboundResponse Transfer(dt_wms0007_reqStockTransferc transfer, 
            string address, string username, string password)
        {
            si_wms0007_outboundClient client =
                new si_wms0007_outboundClient(si_wms0007_outboundClient.EndpointConfiguration.HTTP_Port, address);

            

            client.ClientCredentials.UserName.UserName = username;
            client.ClientCredentials.UserName.Password = password;

            using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
            {
                var httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers[HttpRequestHeader.Authorization] = "Basic " +
                             Convert.ToBase64String(Encoding.ASCII.GetBytes(client.ClientCredentials.UserName.UserName + ":" +
                             client.ClientCredentials.UserName.Password));
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                
                var ret = client.si_wms0007_outboundAsync(
                    new dt_wms0007_reqStockTransferc[] { transfer }).GetAwaiter().GetResult();
                return ret;
            }
        }
    }
}
