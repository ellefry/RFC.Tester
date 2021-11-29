using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SAP.Middleware.Connector;

namespace RFC.Common
{
    public class RfcManager
    {
        public void ProcessRequest(string destinationName, string rfcFunctionName,
            RfcParameter functionParam, RfcParameter headerParam, RfcParameter bodyParams,
           out RfcParameter returnHeaders, out RfcParameter returnBodyStructure, out RfcParameter returnBodyTable,
           string inputHeaderName = "IM_HEADRET", string inputBodyName = "TS_ITEM",
           string returnHeaderName = "EX_HEADRET", string returnBodyName = "EX_RETURN"
        )
        {
            ECCDestinationConfig cfg = null;
            RfcDestination dest = null;
            returnHeaders = new RfcParameter();
            returnBodyStructure = new RfcParameter();
            returnBodyTable = new RfcParameter();

            try
            {
                cfg = new ECCDestinationConfig(new SapConnectionConfig());
                RfcDestinationManager.RegisterDestinationConfiguration(cfg);
                dest = RfcDestinationManager.GetDestination(destinationName);

                var repo = dest.Repository;
                var function = repo.CreateFunction(rfcFunctionName);

                //function params
                foreach (var rfcStructureData in functionParam.Data ?? Enumerable.Empty<RfcStructureData>())
                {
                    function.SetValue(rfcStructureData.Key, rfcStructureData.Value);
                }

                //header params
                if (!string.IsNullOrEmpty(headerParam.StructureName))
                {
                    var headerStructure = function.GetStructure(headerParam.StructureName);
                    foreach (var rfcStructureData in headerParam.Data ?? Enumerable.Empty<RfcStructureData>())
                    {
                        headerStructure.SetValue(rfcStructureData.Key, rfcStructureData.Value);
                    }
                }

                //body params
                if (!string.IsNullOrEmpty(headerParam.StructureName))
                {
                    var items = function.GetTable(bodyParams.StructureName);
                    items.Insert();
                    foreach (var rfcStructureData in bodyParams.Data ?? Enumerable.Empty<RfcStructureData>())
                    {
                        items.CurrentRow.SetValue(rfcStructureData.Key, rfcStructureData.Value);
                    }
                }

                function.Invoke(dest);

                //process returns
                var rfcReturnHeader = function.GetStructure(returnHeaderName);
                for (var liElement = 0; liElement < rfcReturnHeader.ElementCount; liElement++)
                {
                    var element = rfcReturnHeader.GetElementMetadata(liElement);
                    var headerRow = new RfcStructureData { Key =  element.Name, Value = rfcReturnHeader.GetValue(element.Name) };
                    returnHeaders.Data.Add(headerRow);
                }

                //var rfcReturnTable = function.GetTable(returnBodyName)

                RfcSessionManager.EndContext(dest);
                RfcDestinationManager.UnregisterDestinationConfiguration(cfg);
            }
            catch (Exception ex)
            {
                RfcSessionManager.EndContext(dest);
                RfcDestinationManager.UnregisterDestinationConfiguration(cfg);
                Thread.Sleep(1000);
            }
        }
    }
}
