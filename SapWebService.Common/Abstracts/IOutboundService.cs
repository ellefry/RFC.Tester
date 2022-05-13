using System.Threading.Tasks;

namespace SapWebService.Common.Abstracts
{
    public interface IOutboundService
    {
        si_wms0007_outboundResponse Transfer(dt_wms0007_reqStockTransferc transfer,
            string address, string username, string password);
    }
}
