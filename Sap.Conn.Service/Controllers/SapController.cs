using RFC.Common;
using RFC.Common.Interfaces;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sap.Conn.Service.Controllers
{
    public class SapController : ApiController
    {
        private readonly IRfcManager _rfcmanager;

        public SapController(IRfcManager rfcmanager)
        {
            _rfcmanager = rfcmanager;
        }

        [HttpPost]
        public async Task<ProcessRequestInput> ProcessSapRequest(ProcessRequestInput input)
        {
            try
            {
                return await Task.FromResult(_rfcmanager.ProcessRequest(input));
            }
            catch (Exception ex)
            {
                //format message, make length < 1000
                var message = new StringBuilder(ex.Message);
                message.AppendLine(ex.StackTrace);
                if (message.Length > 1000)
                    message.Remove(1000, message.Length - 1000);
                throw new Exception(message.ToString());
            }
        }
     
    }
}