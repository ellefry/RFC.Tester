using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFC.Common
{
    public static class RfcStructureDataExtensions
    {
        public static string GetValue(this RfcParameter parameter, string key)
        {
            return parameter?.Data?.FirstOrDefault(r => r.Key == key)?.Value?.ToString();
        }

        public static string GetTableValue(this RfcParameter parameter, string key)
        {
            return parameter?.TableData?.SelectMany(t => t)?.FirstOrDefault(r => r.Key == key)?.Value?.ToString();
        }

        public static RfcStructureData GetStrcutureData(this RfcParameter parameter, string key)
        {
            return parameter?.Data?.FirstOrDefault(r => r.Key == key);
        }

        public static RfcStructureData GetTableStrcutureData(this RfcParameter parameter, string key)
        {
            return parameter?.TableData?.SelectMany(t => t)?.FirstOrDefault(r => r.Key == key);
        }
    }
}
