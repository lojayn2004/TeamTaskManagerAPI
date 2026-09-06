using System.Net;

namespace TeamTaskManager.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
                if (context.Response.StatusCode == (int)HttpStatusCode.NotFound
                    && !context.Response.HasStarted
                    && context.GetEndpoint() == null)
                {
                    context.Response.ContentType = "application/json";
                    var response = new
                    {
                        message = "Url not found",
                        success = false
                    };
                    await context.Response.WriteAsJsonAsync(response);
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var response = new
                {
                    message = ex.Message,
                    success = false

                };
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
