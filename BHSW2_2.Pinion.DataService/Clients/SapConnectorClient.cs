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
using BHSW2_2.Pinion.DataService.AppServices.Dtos;
using SapWebService.Common;
using SapWebService.Common.Abstracts;
using Microsoft.Extensions.Configuration;

namespace BHSW2_2.Pinion.DataService.Clients
{
    public class SapConnectorClient : ISapConnectorClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<SapConnectorClient> _logger;
        private readonly IOutboundService _outboundService;
        private readonly IConfiguration _configuration;

        public SapConnectorClient(IHttpClientFactory httpClientFactory, ILogger<SapConnectorClient> logger,
            IOutboundService outboundService, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _outboundService = outboundService;
            _configuration = configuration;
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
                        new RfcStructureData{ Key = "PSTNG_DATE", Value = input.PstngDate ?? DateTimeOffset.Now.ToString("yyyyMMdd") },
                        new RfcStructureData{ Key = "OPR_NUM", Value=input.OprNumber },
                    }
                },

                ReturnHeaders = new RfcParameter
                {
                    StructureName = "EX_HEADRET"
                },

                ReturnStructure = new RfcParameter
                {
                    StructureName = "EX_RETURN"
                },
            };
            var content = new StringContent(JsonConvert.SerializeObject(processRequestInput));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await httpClient.PostAsync($"{httpClient.BaseAddress}/sap", content);

            await HandleException(response);

            //process return
            var returnValue = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"Sap finishpart original result: {returnValue}");
            var finishPart = JsonConvert.DeserializeObject<ProcessRequestInput>(returnValue);

            var message = new StringBuilder();
            message.Append($"Sap returns result : [SKU_NUM]: {input.Sku}");
            message.Append($" [MSG_TYP]: {finishPart.ReturnStructure.GetValue("MSG_TYP")}");
            message.Append($" [MSG_TXT1]: {finishPart.ReturnStructure.GetValue("MSG_TXT1")}");
            message.Append($" [MSG_TXT2]: {finishPart.ReturnStructure.GetValue("MSG_TXT2")}");
            message.Append($" [MSG_TXT3]: {finishPart.ReturnStructure.GetValue("MSG_TXT3")}");

            _logger.LogInformation($"Sap FinishPart result: {message}");

            var msgType = finishPart.ReturnStructure.GetStrcutureData("MSG_TYP") ?? throw new NullReferenceException("MSG_TYP");
            if (!"/S/W/I".Contains($"/{msgType.Value?.ToString()}"))
            {
                throw new Exception(message.ToString());
            }
            return message.ToString();
        }

        public async Task<string> ScrapPart(ScrapPartInput input)
        {
            var httpClient = _httpClientFactory.CreateClient(SapConnectorConstants.SapConnectorName);
            ProcessRequestInput processRequestInput = new ProcessRequestInput
            {
                DestinationName = "mySAPdestination",
                RfcFunctionName = "ZMM_DEAL_SCRAP1",
                FunctionParam = new RfcParameter
                {
                    Data = new List<RfcStructureData> {
                        new RfcStructureData{Key = "I_REF_DOC_NO", Value=input.ScrapNumber },
                        new RfcStructureData{Key = "I_PSTNG_DATE", Value= input.PstngDate ?? DateTimeOffset.Now.ToString("yyyyMMdd") },
                        new RfcStructureData{Key = "I_DOC_DATE", Value= input.DocDate ?? DateTimeOffset.Now.ToString("yyyyMMdd")},
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
                    StructureName = "EX_HEADRET"
                },

                ReturnTable = new RfcParameter
                {
                    StructureName = "RETURN"
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

            var message = new StringBuilder();
            message.Append($"Sap returns result : [TYPE]: {scrapPart.ReturnTable.GetTableValue("TYPE")}");
            message.Append($" [MESSAGE]: {scrapPart.ReturnTable.GetTableValue("MESSAGE")}");
            message.Append($" [MESSAGE_V1]: {scrapPart.ReturnTable.GetTableValue("MESSAGE_V1")}");
            message.Append($" [MESSAGE_V2]: {scrapPart.ReturnTable.GetTableValue("MESSAGE_V2")}");
            message.Append($" [MESSAGE_V3]: {scrapPart.ReturnTable.GetTableValue("MESSAGE_V3")}");
            message.Append($" [MESSAGE_V4]: {scrapPart.ReturnTable.GetTableValue("MESSAGE_V4")}");

            _logger.LogInformation($"Sap ScrapPart result: {message}");

            var msgType = scrapPart.ReturnTable.GetTableStrcutureData("TYPE") ?? throw new NullReferenceException("TYPE");
            if (!"/S/W/I".Contains($"/{msgType.Value?.ToString()}"))
            {
                throw new Exception(message.ToString());
            }
            return message.ToString();
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

        public async Task<string> Tranfer(OutboundTransferInput input)
        {
            dt_wms0007_reqStockTransferc transfer = new dt_wms0007_reqStockTransferc();
            transfer.header = new dt_wms0007_reqStockTransfercHeader
            {
                ZWMSNUMBER = input.ZWMSNUMBER,
                ZWMSNO = input.ZWMSNO,
                ZWMSDATE = input.ZWMSDATE ?? DateTimeOffset.Now.ToString("yyyyMMdd"),
                ZWMSOPERATOR = input.ZWMSOPERATOR ?? "MES",
                ZWMSTIME = input.ZWMSTIME ?? DateTimeOffset.Now.ToString("HHmmdd"),
                reservedField = new dt_wms_reserved_st
                {
                    ZRSV01 = string.Empty,
                    ZRSV02 = string.Empty,
                    ZRSV03 = string.Empty,
                    ZRSV04 = string.Empty,
                    ZRSV05 = string.Empty,
                }
            };
            transfer.items = new dt_wms0007_reqStockTransfercItems[] { 
                new dt_wms0007_reqStockTransfercItems
                {
                    ZWMSLINE = string.Empty,
                    MATNR = input.MATNR,
                    MENGE = input.MENGE.ToString(),
                    WERKS = input.WERKS ?? "WH00",
                    LGORT = input.LGORT,
                    SOBKZ = input.SOBKZ ?? string.Empty,
                    LIFNR = input.LIFNR_KDAUF ?? string.Empty,
                    INSMK = input.INSMK ?? string.Empty,
                    UMWRK = input.UMWRK ?? "WH00",
                    UMSOK = input.UMSOK ?? "Z",
                    UMLGO = input.UMLGO ?? "3202",
                    EMLIF = input.EMLIF ?? string.Empty,
                    Zsettlement = input.Zsettlement ?? string.Empty,
                    HULIST = new dt_wms0007_reqStockTransfercItemsHULIST[]{ 
                        new dt_wms0007_reqStockTransfercItemsHULIST{ 
                            EXIDV = input.VEKP_EXIDV,
                            VEMNG= input.VEPO_VEMN.ToString(),
                            reservedField = new dt_wms_reserved_st{ 
                                ZRSV01 = string.Empty,
                                ZRSV02 = string.Empty,
                                ZRSV03 = string.Empty,
                                ZRSV04 = string.Empty,
                                ZRSV05 = string.Empty,
                            }
                        }
                    }
                }
            };
            var address = _configuration.GetValue<string>(SapConnectorConstants.SapOutboudServiceName);
            var username = _configuration.GetValue<string>(SapConnectorConstants.SapUsername);
            var password = _configuration.GetValue<string>(SapConnectorConstants.SapPassword);

            var result = _outboundService.Transfer(transfer, address, username, password);

            var message = new StringBuilder();
            if (result != null && result.mt_wms0007_res != null && result.mt_wms0007_res.Length > 0)
            {
                var response = result.mt_wms0007_res[0];
                message.Append($"Sap outbound transfer result : [TYPE]: {response.ZMESSAGE}");
                message.Append($" [DETAIL]: {response.ZDETIAL}");
                message.Append($" [WMSNUMBER]: {response.ZWMSNUMBER}");
                _logger.LogInformation($"Sap outbound transfer result: {message}");

                if (response.ZMESSAGE?.Trim()?.ToUpper() != "S")
                {
                    throw new Exception(message.ToString());
                }
            }
            else
            {
                throw new Exception("No correct response from server!");
            }
            return message.ToString();
        }
    }
}
