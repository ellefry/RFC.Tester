using RFC.Common;
using System.Threading.Tasks;

namespace Sap.Conn.Service.AppServices.Interfaces
{
    public interface IProcessRequestAppService
    {
        Task ProcessSapRfcRequest(ProcessRequestInput input);
        Task ProcessFailedSapRfcRequest();
    }
}