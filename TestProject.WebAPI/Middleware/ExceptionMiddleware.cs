using System.Net;

namespace TestProject.WebAPI.Middleware
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;

		public ExceptionMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext httpContext, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment environment)
		{
			try
			{
				await _next(httpContext);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Error on {nameof(ExceptionMiddleware)}");
				await HandleExceptionAsync(httpContext, ex, environment);
			}
		}

		private Task HandleExceptionAsync(HttpContext context, Exception exception, IWebHostEnvironment environment)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

			return context.Response.WriteAsync(new
			{
				StatusCode = context.Response.StatusCode,
				Message = "Internal Server Error",
				Error = environment.IsDevelopment() ? exception?.ToString() : null,
			}.ToString());
		}
	}
}
