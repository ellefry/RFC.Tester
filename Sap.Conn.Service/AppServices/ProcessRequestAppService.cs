using RFC.Common.Interfaces;
using Sap.Conn.Service.AppServices.Interfaces;
using Sap.Conn.Service.DataStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using RFC.Common;
using Sap.Conn.Service.Domains;
using Newtonsoft.Json;
using System.Data.Entity;

namespace Sap.Conn.Service.AppServices
{
    public class ProcessRequestAppService : IProcessRequestAppService
    {
        private readonly SapConnectorContext _dbContext;
        private readonly IRfcManager _rfcmanager;

        public ProcessRequestAppService(SapConnectorContext dbContext, IRfcManager rfcmanager)
        {
            _dbContext = dbContext;
            _rfcmanager = rfcmanager;
        }

        public async Task ProcessSapRequest(ProcessRequestInput input)
        {
            try
            {
                _rfcmanager.ProcessRequest(input);
                _dbContext.ProcessRequestHistories.Add(new ProcessRequestHistory
                {
                    Id = Guid.NewGuid(),
                    Content = JsonConvert.SerializeObject(input),
                    Created = DateTimeOffset.Now,
                    FunctionType = FunctionType.RFC,
                    FunctionName = input.RfcFunctionName,
                });
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _dbContext.ProcessRequests.Add(new ProcessRequest { 
                    Id = Guid.NewGuid(),
                    Content = JsonConvert.SerializeObject(input),
                    Created = DateTimeOffset.Now,
                    FunctionType = FunctionType.RFC,
                    FunctionName = input.RfcFunctionName,
                    Retries = 0,
                    Error = ex.Message
                });
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task ProcessFailedSapRequest(ProcessRequest processRequest)
        {
            try
            {
                var sapRequest = JsonConvert.DeserializeObject<ProcessRequestInput>(processRequest.Content);
                _rfcmanager.ProcessRequest(sapRequest);
                _dbContext.ProcessRequestHistories.Add(new ProcessRequestHistory
                {
                    Id = Guid.NewGuid(),
                    Content = JsonConvert.SerializeObject(processRequest.Content),
                    Created = DateTimeOffset.Now,
                    FunctionType = processRequest.FunctionType,
                    FunctionName = processRequest.FunctionName,
                });
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var pr = _dbContext.ProcessRequests.FirstOrDefault(p => p.Id == processRequest.Id);
                pr.Error = ex.Message;
                pr.Modified = DateTimeOffset.Now;
                pr.Retries += 1;
                _dbContext.ProcessRequests.Attach(pr);
                _dbContext.Entry(pr).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}