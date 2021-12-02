using SAP.Middleware.Connector;

namespace RFC.Common.Interfaces
{
    public interface IRfcFunctionOperator
    {
        IRfcFunction Create(string functionName, RfcRepository repo);
        bool Execute(IRfcFunction function, RfcDestination rfcDestination);
    }
}
