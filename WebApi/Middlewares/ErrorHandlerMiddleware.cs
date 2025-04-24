using Application.Exceptions;
using Application.Wrappers;
using System.Net;
using System.Text.Json;

namespace WebApi.Middlewares
{
	public class ErrorHandlerMiddleware
	{
        private readonly RequestDelegate _next;
		public ErrorHandlerMiddleware(RequestDelegate next)
		{
			_next = next;
		}
		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				var response = context.Response;
				response.ContentType = "application/json";
				var responseModel = new ApiResponse<string>
				{
					Succecced=false,
					Message=ex.Message,
				};
				switch (ex)
				{
					case ApiException apiException:
						response.StatusCode = (int)HttpStatusCode.BadRequest; 
						break;
					case ValidationErrorException validationErrorException:
						response.StatusCode = (int)HttpStatusCode.BadRequest;
						responseModel.Errors = validationErrorException.Errors;
						break;
					default:
						response.StatusCode = (int)HttpStatusCode.InternalServerError;
						break;
				}
				var result=JsonSerializer.Serialize(responseModel);
				await response.WriteAsync(result);
			}
		}
	}
}
