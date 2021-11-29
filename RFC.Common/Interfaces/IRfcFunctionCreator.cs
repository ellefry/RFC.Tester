using SAP.Middleware.Connector;

namespace RFC.Common.Interfaces
{
    public interface IRfcFunctionCreator
    {
        IRfcFunction Create(string functionName, RfcRepository repo);
    }
}
