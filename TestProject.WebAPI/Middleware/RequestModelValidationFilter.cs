using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace TestProject.WebAPI.Middleware
{
	public class RequestModelValidationFilter : IActionFilter
	{
		private readonly ILogger<RequestModelValidationFilter> _logger;

		public RequestModelValidationFilter(ILogger<RequestModelValidationFilter> logger)
		{
			_logger = logger;
		}
		public void OnActionExecuted(ActionExecutedContext context)
		{
		}

		public void OnActionExecuting(ActionExecutingContext context)
		{
			if (!context.ModelState.IsValid)
			{
				context.Result = new BadRequestObjectResult(context.ModelState);
                _logger.LogWarning(JsonSerializer.Serialize(context.ModelState));
			}
		}
	}
}
