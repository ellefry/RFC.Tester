using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFC.Common.Interfaces
{
  public interface IRfcManager
  {
        void ProcessRequest(string destinationName, string rfcFunctionName,
            RfcParameter functionParam, RfcParameter headerParam, RfcParameter tableParams,
            RfcParameter returnHeaders, RfcParameter returnStructure, RfcParameter returnTable
        );
  }
}
