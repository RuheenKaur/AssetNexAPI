

using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace AssetNex.API.Models.DomainModelsANI
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                statusCode = 500,
                message = "An unexpected error occurred.",
                detail = ex.Message
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}

//public class GlobalExceptionHandler : IExceptionHandler
//{
//    public async ValueTask<bool> TryHandleAsync(HttpContext context, 
//        Exception exception, CancellationToken cancellationToken)
//    {
//        var problemDetails = new ProblemDetails();
//        {
//            Status = StatusCodes.Status500InternalServerError,
//            Title = "An unexpected error occurred",
//            Detail = exception.Message
//        };

//        context.Response.StatusCode = 500;
//        await context.Response.WriteAsJsonAsync(problemDetails);
//        return true;
//    }
//}
