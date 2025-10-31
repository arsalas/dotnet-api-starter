using System.Net;
using System.Text.Json;
using ApiStarter.Exceptions;

namespace ApiStarter.Middleware;

public class ExceptionHandlingMiddleware(
		RequestDelegate next,
		ILogger<ExceptionHandlingMiddleware> logger)
{
	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await next(context);
		}
		catch (Exception ex)
		{
			await HandleExceptionAsync(context, ex);
		}
	}

	private async Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		context.Response.ContentType = "application/json";

		var (statusCode, message) = exception switch
		{
			NotFoundException => (HttpStatusCode.NotFound, exception.Message),
			ValidationException => (HttpStatusCode.BadRequest, exception.Message),
			UnauthorizedException => (HttpStatusCode.Unauthorized, exception.Message),
			_ => (HttpStatusCode.InternalServerError, "Error interno del servidor")
		};

		context.Response.StatusCode = (int)statusCode;

		if (statusCode == HttpStatusCode.InternalServerError)
		{
			logger.LogError(exception, "Error no controlado: {Message}", exception.Message);
		}
		else
		{
			logger.LogWarning("Error {StatusCode}: {Message}", statusCode, exception.Message);
		}

		var response = new
		{
			statusCode = (int)statusCode,
			message = message
		};

		await context.Response.WriteAsJsonAsync(response);
	}
}