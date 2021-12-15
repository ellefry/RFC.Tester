using Newtonsoft.Json;
using RFC.Common;
using RFC.Common.Interfaces;
using Sap.Conn.Service.AppServices.Interfaces;
using Sap.Conn.Service.DataStorage;
using Sap.Conn.Service.Domains;
using Sap.Conn.Service.Domains.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Sap.Conn.Service.AppServices
{
    public class ProcessRequestAppService : IProcessRequestAppService
    {
        private readonly SapConnectorContext _dbContext;
        private readonly IRfcManager _rfcmanager;
        private readonly ISapSwitcher _sapSwitcher;

        public ProcessRequestAppService(SapConnectorContext dbContext, IRfcManager rfcmanager, ISapSwitcher sapSwitcher)
        {
            _dbContext = dbContext;
            _rfcmanager = rfcmanager;
            _sapSwitcher = sapSwitcher;
        }

        public async Task ProcessSapRfcRequest(ProcessRequestInput input)
        {
            try
            {
                if (!_sapSwitcher.IsEnabled)
                {
                    await SaveFailedSapRequestLog(JsonConvert.SerializeObject(input), FunctionType.RFC,
                        input.RfcFunctionName, $"Sap switcher is off");
                    return;
                }
                _rfcmanager.ProcessRequest(input);
                await SaveSapRequestLog(input, FunctionType.RFC, input.RfcFunctionName);
            }
            catch (Exception ex)
            {
                await SaveFailedSapRequestLog(JsonConvert.SerializeObject(input), FunctionType.RFC,
                    input.RfcFunctionName, $"{ex.GetType()} : {ex.Message}");
            }
        }

        public async Task ProcessFailedSapRfcRequest()
        {
            if (!_sapSwitcher.IsEnabled)
                return;

            var failedRequests = _dbContext.ProcessRequests.ToList();
            foreach (var failedRequest in failedRequests)
            {
                await ProcessFailedSapRfcRequest(failedRequest);
            }
        }

        private async Task ProcessFailedSapRfcRequest(ProcessRequest processRequest)
        {
            try
            {
                var sapRequest = JsonConvert.DeserializeObject<ProcessRequestInput>(processRequest.Content);
                _rfcmanager.ProcessRequest(sapRequest);
                await SaveSapRequestLog(processRequest.Content, processRequest.FunctionType, processRequest.FunctionName);
            }
            catch (Exception ex)
            {
                var pr = _dbContext.ProcessRequests.FirstOrDefault(p => p.Id == processRequest.Id);
                pr.Error = ex.Message;
                pr.Modified = DateTimeOffset.Now;
                pr.Retries += 1;
                _dbContext.Entry(pr).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
        }

        private async Task SaveFailedSapRequestLog(string requestBody, FunctionType functionType,
            string functionName, string error)
        {
            _dbContext.ProcessRequests.Add(new ProcessRequest
            {
                Id = Guid.NewGuid(),
                Content = requestBody,
                Created = DateTimeOffset.Now,
                FunctionType = functionType,
                FunctionName = functionName,
                Retries = 0,
                Error = error
            });
            await _dbContext.SaveChangesAsync();
        }

        private async Task SaveSapRequestLog<T>(T body, FunctionType functionType, string functionName)
        {
            _dbContext.ProcessRequestHistories.Add(new ProcessRequestHistory
            {
                Id = Guid.NewGuid(),
                Content = JsonConvert.SerializeObject(body),
                Created = DateTimeOffset.Now,
                FunctionType = FunctionType.RFC,
                FunctionName = functionName,
            });
            await _dbContext.SaveChangesAsync();
        }


    }
}