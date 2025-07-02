namespace NewsAggregation.Middlewares
{
    public class ContentTypeVaildation
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ContentTypeVaildation> _logger;
        public ContentTypeVaildation(RequestDelegate next, ILogger<ContentTypeVaildation> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            _logger.LogInformation("Checking Content Type In middleware");
            //if (httpContext.Request.Headers.ContainsKey("Content-Type"))
            //{
            //    if (httpContext.Request.Headers.TryGetValue("Content-Type", out var contentType))
            //    {
            //        if (contentType != "application/json")
            //        {
            //            httpContext.Response.StatusCode = 401;
            //            return;
            //        }
            //    }
            //}
            await _next(httpContext);
        }
    }
}