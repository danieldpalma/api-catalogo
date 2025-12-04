using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiCatalogo.Filters;

public class ApiExceptionFilter : IExceptionFilter
{
	private readonly ILogger<ApiExceptionFilter> _logger;
	public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
	{
		_logger = logger;
	}
	public void OnException(ExceptionContext context)
	{
		_logger.LogError(context.Exception, message: $"An unhandled exception occurred: {StatusCodes.Status500InternalServerError}");

		context.Result = new ObjectResult("An error occurred while processing your request: Status Code " + StatusCodes.Status500InternalServerError)
		{
			StatusCode = StatusCodes.Status500InternalServerError
		};
	}
}
