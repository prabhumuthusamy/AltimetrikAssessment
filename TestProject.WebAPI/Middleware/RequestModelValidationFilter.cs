using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace TestProject.WebAPI.Middleware
{
	public class RequestModelValidationFilter : IActionFilter
	{
		public void OnActionExecuted(ActionExecutedContext context)
		{
		}

		public void OnActionExecuting(ActionExecutingContext context)
		{
			if (!context.ModelState.IsValid)
			{
				context.Result = new BadRequestObjectResult(context.ModelState);
			}
		}
	}
}
