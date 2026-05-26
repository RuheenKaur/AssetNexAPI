namespace AssetNex.API.Models.DomainModel
{
    class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RequestLoggingMiddleware _nextt;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILogger<RequestLoggingMiddleware> logger)
        {
            logger.LogInformation($"Incoming Request: {context.Request.Method} {context.Request.Path}");
            await _next(context);
            logger.LogInformation($"Outgoing Response: {context.Response.StatusCode}");


        }
    }
}
