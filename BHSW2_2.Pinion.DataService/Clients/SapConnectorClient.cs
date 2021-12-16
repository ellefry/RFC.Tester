using BHSW2_2.Pinion.DataService.Clients.Dtos;
using RFC.Common;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BHSW2_2.Pinion.DataService.Clients
{
    public class SapConnectorClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SapConnectorClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory=httpClientFactory;
        }

        public async Task FinishPart(FinishPartInput input)
        {
            var httpClient = _httpClientFactory.CreateClient("Sap.Conn.Service");
            ProcessRequestInput processRequestInput = new ProcessRequestInput { 
                DestinationName = "mySAPdestination",
                RfcFunctionName = "FunctionZPP_REPMANCONF1_CREATE_MTS",
                headerParam  = new RfcParameter { 
                    StructureName = "IM_HEADRET",
                    Data  = new List<RfcStructureData> {
                        new RfcStructureData{ Key = "PLANT", Value=input.Plant },
                        new RfcStructureData{ Key = "MATERIAL", Value=input.Material },
                        new RfcStructureData{ Key = "BACKFL_QUANT", Value=input.Quantity },
                        new RfcStructureData{ Key = "SKU_NUM", Value=input.Sku },
                        new RfcStructureData{ Key = "Z_GZKH", Value="" },
                        new RfcStructureData{ Key = "PROD_VER", Value=input.ProductVersion },
                        new RfcStructureData{ Key = "PSTNG_DATE", Value="YYYYMMDD" },
                        new RfcStructureData{ Key = "OPR_NUM", Value=input.OprNumber },
                    }
                },
                
                returnHeaders = new RfcParameter
                {
                    StructureName = "EX_HEADRET",
                    Data  = new List<RfcStructureData> {
                        new RfcStructureData{ Key = "MAT_DOC" },
                        new RfcStructureData{ Key = "DOC_YEAR"},
                        new RfcStructureData{ Key = "PSTNG_DATE" },
                        new RfcStructureData{ Key = "DOC_DATE" },
                        new RfcStructureData{ Key = "PR_UNAME" },
                    }
                },

                returnStructure = new RfcParameter
                {
                    StructureName = "EX_RETURN",
                    Data  = new List<RfcStructureData> {
                        new RfcStructureData{ Key = "MSG_TYP", TargetValue = "S" },
                        new RfcStructureData{ Key = "WERKS"},
                        new RfcStructureData{ Key = "MATNR" },
                        new RfcStructureData{ Key = "MAKTX" },
                        new RfcStructureData{ Key = "Z_ERFMG" },
                        new RfcStructureData{ Key = "Z_SKU" },
                        new RfcStructureData{ Key = "VERID" },
                        new RfcStructureData{ Key = "Z_DATUM" },
                        new RfcStructureData{ Key = "MSG_TXT1" },
                        new RfcStructureData{ Key = "MSG_TXT2" },
                        new RfcStructureData{ Key = "MSG_TXT3" },
                    }
                },
            };
            var content = new StringContent(JsonConvert.SerializeObject(processRequestInput));
            var response = await httpClient.PostAsync($"{httpClient.BaseAddress}/sap", content);
            if (!response.IsSuccessStatusCode)
            {
                System.Console.WriteLine("FinishPart error");
            }
        }
    }
}
