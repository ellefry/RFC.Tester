using System.Collections.Generic;

namespace RFC.Common
{
   
    public class RfcParameter
    {
        public string StructureName { get; set; }
        public List<RfcStructureData> Data { get; set; } = new List<RfcStructureData>();
        public List<List<RfcStructureData>> TableData { get; set; } = new List<List<RfcStructureData>>();
    }

    public class RfcStructureData
    {
        public string Key { get; set; }
        public object Value { get; set; }
    }

}
