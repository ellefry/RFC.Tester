using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFC.Common
{
    public class ProcessRequestInput
    {
        public string DestinationName { get; set; } = "mySAPdestination";
        public string RfcFunctionName { get; set; }
        public RfcParameter FunctionParam { get; set; }
        public RfcParameter HeaderParam { get; set; }
        public RfcParameter TableParams { get; set; }
        public RfcParameter ReturnHeaders { get; set; }
        public RfcParameter ReturnStructure { get; set; }
        public RfcParameter ReturnTable { get; set; }
    }
}
