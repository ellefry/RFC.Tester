using System.Threading.Tasks;

namespace BHSW2_2.Pinion.DataService.FailureHandlers.Interfaces
{
    public interface ISapServiceHandler
    {
        Task Handle(SapRequest sapRequest);
    }
}
