namespace JetStreamApiMongoDb.Middleware
{
    /// <summary>
    /// Middleware for exception handling
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invoke method for exception handling
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {

                if (!context.Response.HasStarted)
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                    await context.Response.WriteAsJsonAsync(new
                    {
                        Message = "Ein interner Serverfehler ist aufgetreten.",
                        StatusCode = context.Response.StatusCode,
                        Detail = ex.Message
                    });
                }
            }
            if ((context.Response.StatusCode == 401 || context.Response.StatusCode == 403) &&
                context.Response.ContentLength == null &&
                !context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new
                {
                    Message = "Zugriff verweigert: Sie haben keine Berechtigung.",
                    StatusCode = context.Response.StatusCode
                });
            }
        }
    }
}
