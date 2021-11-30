using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFC.Common
{
    public class ProcessRequestInput
    {
        public string DestinationName { get; set; }
        public string RfcFunctionName { get; set; }
        public RfcParameter functionParam { get; set; }
        public RfcParameter headerParam { get; set; }
        public RfcParameter tableParams { get; set; }
        public RfcParameter returnHeaders { get; set; }
        public RfcParameter returnStructure { get; set; }
        public RfcParameter returnTable { get; set; }
    }
}
