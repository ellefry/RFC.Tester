using BHSW2_2.Pinion.DataService.Clients.Dtos;
using RFC.Common;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;
using System;
using BHSW2_2.Pinion.DataService.Clients.Abstracts;
using System.Net.Http.Headers;

namespace BHSW2_2.Pinion.DataService.Clients
{
    public class SapConnectorClient : ISapConnectorClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SapConnectorClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
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
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await httpClient.PostAsync($"{httpClient.BaseAddress}/sap", content);

            await HandleException(response);

            //process return
            var returnValue = await response.Content.ReadAsStringAsync();
            var finishPartResponse = JsonConvert.DeserializeObject<ProcessRequestInput>(returnValue);

        }

        private async Task HandleException(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = new StringBuilder("SapConnectorClient :");
                errorMessage.AppendLine($"[Http Request Error]");
                errorMessage.AppendLine($"[Request Url]: {response.RequestMessage.RequestUri}");
                errorMessage.AppendLine($"[Status]: {response.StatusCode}");
                errorMessage.AppendLine($"[Headers]: {response.Headers}");
                errorMessage.AppendLine($"[Body]: {await response.Content.ReadAsStringAsync()}");
                throw new Exception(errorMessage.ToString());
            }
            await Task.CompletedTask;
        }
    }
}
