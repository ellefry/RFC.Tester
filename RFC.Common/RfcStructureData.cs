using System.Collections.Generic;

namespace RFC.Common
{
   
    public class RfcParameter
    {
        public string StructureName { get; set; }
        public RFCParameterType ParameterType { get; set; } = RFCParameterType.Structure;
        public List<RfcStructureData> Data { get; set; } = new List<RfcStructureData>();
    }

    public class RfcStructureData
    {
        public string Key { get; set; }
        public dynamic Value { get; set; }
    }


    public enum RFCParameterType
    {
        Structure,
        Table
    }
}
