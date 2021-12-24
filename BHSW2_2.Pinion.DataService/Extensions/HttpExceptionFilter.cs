using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading.Tasks;

namespace BHSW2_2.Pinion.DataService.Extensions
{
    public class HttpExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<HttpExceptionFilter> _logger;

        public HttpExceptionFilter(ILogger<HttpExceptionFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            context.ExceptionHandled = true;

            context.HttpContext.Response.StatusCode = 500;

            var message = new StringBuilder();
            message.AppendLine("Message ---\n{0}" + context.Exception.Message);
            message.AppendLine("Source ---\n{0}" + context.Exception.Source);
            message.AppendLine("StackTrace ---\n{0}" + context.Exception.StackTrace);
            if (message.Length > 1000)
                message.Remove(1000, message.Length - 1000);
            _logger.LogError(message.ToString());
            var resp = new ResponseResult<bool>(false);
            resp.Success = false;
            resp.Message = message.ToString();
            context.Result = new JsonResult(resp);
            await Task.CompletedTask;
        }
    }
}
