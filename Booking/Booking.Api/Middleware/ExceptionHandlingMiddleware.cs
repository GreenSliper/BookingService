using Booking.Application.Exceptions;
using Booking.Domain.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Booking.Api.Middleware
{
	public sealed class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionHandlingMiddleware> _logger;

		public ExceptionHandlingMiddleware(
			RequestDelegate next,
			ILogger<ExceptionHandlingMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unhandled exception");

				await HandleExceptionAsync(context, ex);
			}
		}

		private static Task HandleExceptionAsync(HttpContext context, Exception ex)
		{
			var (status, message) = ex switch
			{
				ValidationException => (StatusCodes.Status400BadRequest, ex.Message),
				DomainException => (StatusCodes.Status400BadRequest, ex.Message),
				NotFoundException => (StatusCodes.Status404NotFound, ex.Message),
				ForbiddenException => (StatusCodes.Status403Forbidden, ex.Message),

				_ => (StatusCodes.Status500InternalServerError, "Internal server error")
			};

			context.Response.StatusCode = status;
			context.Response.ContentType = "application/json";

			return context.Response.WriteAsJsonAsync(new
			{
				error = message
			});
		}
	}

}
