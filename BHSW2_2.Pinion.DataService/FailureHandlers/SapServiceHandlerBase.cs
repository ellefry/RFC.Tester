using BHSW2_2.Pinion.DataService.FailureHandlers.Interfaces;
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

        public abstract Task Handle(SapRequest sapRequest);

        protected async Task MoveToHistory(SapRequest sapRequest)
        {
            var history = new SapRequestHistory
            {
                Id = Guid.NewGuid(),
                Content = sapRequest.Content,
                FunctionName = sapRequest.FunctionName,
                Created = DateTimeOffset.Now
            };
            _dbContext.SapRequestHistories.Add(history);
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
