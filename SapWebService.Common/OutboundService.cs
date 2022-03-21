using SapWebService.Common.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SapWebService.Common
{
    public class OutboundService : IOutboundService
    {
        public async Task Transfer()
        {
            si_wms0007_outboundClient client =
                new si_wms0007_outboundClient(si_wms0007_outboundClient.EndpointConfiguration.HTTP_Port,
               "http://poqas:50000/XISOAPAdapter/MessageServlet?senderParty=&senderService=BS_WMS" +
                        "_QAS&receiverParty=&receiverService=&interface=si_wms0007_outbound&interfaceName" +
                        "space=http%3A%2F%2Fwww.boschhuayu.com%2Fwms");

            var ret = await client.si_wms0007_outboundAsync(
                new dt_wms0007_reqStockTransferc[] { new dt_wms0007_reqStockTransferc() });
            await Task.CompletedTask;
        }
    }
}
