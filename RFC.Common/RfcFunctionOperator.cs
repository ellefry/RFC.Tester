using RFC.Common.Interfaces;
using SAP.Middleware.Connector;
using System;

namespace RFC.Common
{
    public class RfcFunctionOperator : IRfcFunctionOperator
    {
        public IRfcFunction Create(string functionName, RfcRepository repo)
        {
            var function = repo.CreateFunction(functionName);
            return function;
        }

        public bool Execute(IRfcFunction function, RfcRepoWrapper repoWrapper)
        {
            function.Invoke(repoWrapper.Destination);
            return true;
        }
    }

}
