using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
namespace ResourceManageGroup.Exceptions
{
    public class CustomExceptionFilter : Attribute, IExceptionFilter
    {
        private readonly ILogger<CustomExceptionFilter> _logger;
        public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
        {
            _logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "An exception occurred.");
        }
    }
}