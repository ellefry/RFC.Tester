using BHSW2_2.Pinion.DataService.FailureHandlers.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace BHSW2_2.Pinion.DataService.FailureHandlers
{
    public abstract class SapServiceHandlerBase : ISapServiceHandler
    {
        protected readonly SapConnectorContext _dbContext;
        protected SapServiceHandlerBase(SapConnectorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(SapRequest sapRequest)
        {
            try
            {
                var message = await ProcessSapRequest(sapRequest);
                await MoveToHistory(sapRequest, message);
            }
            catch (Exception ex)
            {
                sapRequest.UpdateRetry($"{ex.Message}  {ex.StackTrace}");
                _dbContext.Entry(sapRequest).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
        }

        protected abstract Task<string> ProcessSapRequest(SapRequest sapRequest);

        protected async Task MoveToHistory(SapRequest sapRequest, string message)
        {
            var history = new SapRequestHistory
            {
                Id = Guid.NewGuid(),
                Content = sapRequest.Content,
                FunctionName = sapRequest.FunctionName,
                SapMessage = message,
                Created = DateTimeOffset.Now
            };
            _dbContext.SapRequestHistories.Add(history);
            _dbContext.SapRequests.Remove(sapRequest);
            await _dbContext.SaveChangesAsync();
        }

        protected async Task HandleException<T>(T content, string functionName, string error)
        {
            var sapRequest = new SapRequest
            {
                Content = JsonConvert.SerializeObject(content),
                FunctionName = functionName,
                Error = error,
                Created = DateTimeOffset.Now
            };
            _dbContext.SapRequests.Add(sapRequest);
            await _dbContext.SaveChangesAsync();
        }
    }
}
