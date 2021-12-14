using RFC.Common;
using Sap.Conn.Service.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Sap.Conn.Service.AppServices.Interfaces
{
    public interface IProcessRequestAppService
    {
        Task ProcessSapRequest(ProcessRequestInput input);
        Task ProcessFailedSapRequest(ProcessRequest processRequest);
    }
}