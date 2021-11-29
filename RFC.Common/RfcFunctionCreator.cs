using RFC.Common.Interfaces;
using SAP.Middleware.Connector;
using System;

namespace RFC.Common
{
    public class RfcFunctionCreator : IRfcFunctionCreator
    {
        public IRfcFunction Create(string functionName, RfcRepository repo)
        {
            var function = repo.CreateFunction(functionName);
            return function;
        }

    }
}
