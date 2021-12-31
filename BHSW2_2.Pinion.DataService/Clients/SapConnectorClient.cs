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
using System.Linq;
using Microsoft.Extensions.Logging;

namespace BHSW2_2.Pinion.DataService.Clients
{
    public class SapConnectorClient : ISapConnectorClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<SapConnectorClient> _logger;

        public SapConnectorClient(IHttpClientFactory httpClientFactory, ILogger<SapConnectorClient> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<string> FinishPart(FinishPartInput input)
        {
            var httpClient = _httpClientFactory.CreateClient(SapConnectorConstants.SapConnectorName);
            ProcessRequestInput processRequestInput = new ProcessRequestInput
            {
                DestinationName = "mySAPdestination",
                RfcFunctionName = "ZPP_REPMANCONF1_CREATE_MTS",
                HeaderParam = new RfcParameter
                {
                    StructureName = "IM_HEADRET",
                    Data = new List<RfcStructureData> {
                        new RfcStructureData{ Key = "PLANT", Value=input.Plant },
                        new RfcStructureData{ Key = "MATERIAL", Value=input.Material },
                        new RfcStructureData{ Key = "BACKFL_QUANT", Value=input.Quantity },
                        new RfcStructureData{ Key = "SKU_NUM", Value=input.Sku },
                        new RfcStructureData{ Key = "Z_GZKH", Value="" },
                        new RfcStructureData{ Key = "PROD_VER", Value=input.ProductVersion },
                        new RfcStructureData{ Key = "PSTNG_DATE", Value=DateTimeOffset.Now.ToString("yyyyMMdd") },
                        new RfcStructureData{ Key = "OPR_NUM", Value=input.OprNumber },
                    }
                },

                ReturnHeaders = new RfcParameter
                {
                    StructureName = "EX_HEADRET",
                    //Data = new List<RfcStructureData> {
                    //    new RfcStructureData{ Key = "MAT_DOC" },
                    //    new RfcStructureData{ Key = "DOC_YEAR"},
                    //    new RfcStructureData{ Key = "PSTNG_DATE" },
                    //    new RfcStructureData{ Key = "DOC_DATE" },
                    //    new RfcStructureData{ Key = "PR_UNAME" },
                    //}
                },

                ReturnStructure = new RfcParameter
                {
                    StructureName = "EX_RETURN",
                    //Data = new List<RfcStructureData> {
                    //    new RfcStructureData{ Key = "MSG_TYP", TargetValue = "/S/W/I" },
                    //    new RfcStructureData{ Key = "WERKS"},
                    //    new RfcStructureData{ Key = "MATNR" },
                    //    new RfcStructureData{ Key = "MAKTX" },
                    //    new RfcStructureData{ Key = "Z_ERFMG" },
                    //    new RfcStructureData{ Key = "Z_SKU" },
                    //    new RfcStructureData{ Key = "VERID" },
                    //    new RfcStructureData{ Key = "Z_DATUM" },
                    //    new RfcStructureData{ Key = "MSG_TXT1" },
                    //    new RfcStructureData{ Key = "MSG_TXT2" },
                    //    new RfcStructureData{ Key = "MSG_TXT3" },
                    //}
                },
            };
            var content = new StringContent(JsonConvert.SerializeObject(processRequestInput));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await httpClient.PostAsync($"{httpClient.BaseAddress}/sap", content);

            await HandleException(response);

            //process return
            var returnValue = await response.Content.ReadAsStringAsync();
            var finishPart = JsonConvert.DeserializeObject<ProcessRequestInput>(returnValue);

            var message = $"Sap returns result : [MSG_TYP]: {finishPart.ReturnStructure.GetValue("MSG_TYP")}" +
                   $" [MSG_TXT1]: {finishPart.ReturnStructure.GetValue("MSG_TXT1")} " +
                   $" [MSG_TXT2]: {finishPart.ReturnStructure.GetValue("MSG_TX2")} " +
                   $" [MSG_TXT3]: {finishPart.ReturnStructure.GetValue("MSG_TXT3")}";
            _logger.LogInformation($"Sap FinishPart result: {message}");

            var msgType = finishPart.ReturnStructure.GetStrcutureData("MSG_TYP") ?? throw new NullReferenceException("MSG_TYP");
            if (!msgType.TargetValue.ToString().Contains($"/{msgType.Value?.ToString()}"))
            {
                throw new Exception(message);
            }
            return message;
        }

        public async Task<string> ScrapPart(ScrapPartInput input)
        {
            var httpClient = _httpClientFactory.CreateClient(SapConnectorConstants.SapConnectorName);
            ProcessRequestInput processRequestInput = new ProcessRequestInput
            {
                DestinationName = "mySAPdestination",
                RfcFunctionName = "ZMM_DEAL_SCRAP",
                FunctionParam = new RfcParameter
                {
                    Data = new List<RfcStructureData> {
                        new RfcStructureData{Key = "I_REF_DOC_NO", Value=input.ScrapNumber },
                        new RfcStructureData{Key = "I_PSTNG_DATE", Value= DateTimeOffset.Now.ToString("yyyyMMdd") },
                        new RfcStructureData{Key = "I_DOC_DATE", Value=DateTimeOffset.Now.ToString("yyyyMMdd")},
                        new RfcStructureData{Key = "I_MATERIAL", Value=input.ParentMaterial },
                        new RfcStructureData{Key = "I_VORNR", Value=input.Vornr },
                    }
                },
                TableParams = new RfcParameter
                {
                    StructureName = "TS_ITEM",
                    Data = new List<RfcStructureData> {
                        new RfcStructureData{ Key = "MATERIAL", Value=input.Material },
                        new RfcStructureData{ Key = "PLANT", Value=input.Plant },
                        new RfcStructureData{ Key = "STGE_LOC", Value=input.StorageLocation },
                        new RfcStructureData{ Key = "ENTRY_QNT", Value=input.Quantity },
                        new RfcStructureData{ Key = "MOVE_REAS", Value=input.ScrapReason },
                    }
                },

                ReturnHeaders = new RfcParameter
                {
                    StructureName = "EX_HEADRET",
                    //Data = new List<RfcStructureData> {
                    //    new RfcStructureData{ Key = "MAT_DOC" },
                    //    new RfcStructureData{ Key = "DOC_YEAR"},
                    //    new RfcStructureData{ Key = "REF_DOC_NO"},
                    //    new RfcStructureData{ Key = "PSTNG_DATE" },
                    //    new RfcStructureData{ Key = "DOC_DATE" },
                    //    new RfcStructureData{ Key = "PR_UNAME" },
                    //    new RfcStructureData{ Key = "HEADER_TXT"},
                    //     new RfcStructureData{ Key = "VORNR"},
                    //}
                },

                ReturnTable = new RfcParameter
                {
                    StructureName = "RETURN",
                    //Data = new List<RfcStructureData> {
                    //    new RfcStructureData{ Key = "TYPE", TargetValue = "/S/W/I" },
                    //    new RfcStructureData{ Key = "ID"},
                    //    new RfcStructureData{ Key = "NUMBER" },
                    //    new RfcStructureData{ Key = "MESSAGE" },
                    //    new RfcStructureData{ Key = "LOG_NO" },
                    //    new RfcStructureData{ Key = "LOG_MSG_NO" },
                    //    new RfcStructureData{ Key = "MESSAGE_V1" },
                    //    new RfcStructureData{ Key = "MESSAGE_V2" },
                    //    new RfcStructureData{ Key = "MESSAGE_V3" },
                    //    new RfcStructureData{ Key = "MESSAGE_V4" },
                    //    new RfcStructureData{ Key = "PARAMETER" },
                    //    new RfcStructureData{ Key = "ROW" },
                    //    new RfcStructureData{ Key = "FIELD" },
                    //    new RfcStructureData{ Key = "SYSTEM" },

                    //}
                },
            };
            var content = new StringContent(JsonConvert.SerializeObject(processRequestInput));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await httpClient.PostAsync($"{httpClient.BaseAddress}/sap", content);

            await HandleException(response);

            //process return
            var returnValue = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"Sap ScrapPart original result: {returnValue}");

            var scrapPart = JsonConvert.DeserializeObject<ProcessRequestInput>(returnValue);

            var message = $"Sap returns result : [TYPE]: {scrapPart.ReturnTable.GetTableValue("TYPE")}" +
                    $" [MESSAGE_V1]: {scrapPart.ReturnTable.GetTableValue("MESSAGE_V1")} " +
                    $" [MESSAGE_V2]: {scrapPart.ReturnTable.GetTableValue("MESSAGE_V2")} " +
                    $" [MESSAGE_V3]:{scrapPart.ReturnTable.GetTableValue("MESSAGE_V3")}" +
                    $" [MESSAGE_V4]:{scrapPart.ReturnTable.GetTableValue("MESSAGE_V4")}";
            _logger.LogInformation($"Sap ScrapPart result: {message}");

            var msgType = scrapPart.ReturnTable.GetTableStrcutureData("TYPE") ?? throw new NullReferenceException("TYPE");
            if (!msgType.TargetValue.ToString().Contains($"/{msgType.Value?.ToString()}"))
            {
                throw new Exception(message);
            }
            return message;
        }

        private async Task HandleException(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = new StringBuilder();
                errorMessage.AppendLine($"[Http Request Error]");
                errorMessage.AppendLine($"[Request Url]: {response.RequestMessage.RequestUri}");
                errorMessage.AppendLine($"[Status]: {response.StatusCode}");
                errorMessage.AppendLine($"[Body]: {await response.Content.ReadAsStringAsync()}");
                throw new Exception(errorMessage.ToString());
            }
            await Task.CompletedTask;
        }
    }
}
