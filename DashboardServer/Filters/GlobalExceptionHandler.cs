using System.Net;
using DashboardServer.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticAssets;

namespace DashboardServer.Filters
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Unhandled exception occurred");

            var (statusCode, title) = MapExceptionToResponse(exception);

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = exception.Message
            };

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }

        private static (int StatusCode, string Title) MapExceptionToResponse(Exception exception)
        {
            return exception switch
            {
                UnauthorizedException => (StatusCodes.Status401Unauthorized, "Unauthorized"),
                BadRequestException => (StatusCodes.Status400BadRequest, "Bad Request"),
                NotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
                InvalidOperationException => (StatusCodes.Status400BadRequest, "Invalid Operation"),
                _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
            };
        }
    }
}
