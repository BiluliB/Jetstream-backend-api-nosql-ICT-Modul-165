namespace JetStreamApiMongoDb.Middleware
{
    public class StatusCodeHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public StatusCodeHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.ContentLength == null)
            {
                var message = string.Empty;
                var statusCode = context.Response.StatusCode;

                switch (statusCode)
                {
                    case StatusCodes.Status400BadRequest:
                        message = "Bad Request.";
                        break;
                    case StatusCodes.Status401Unauthorized:
                        message = "Zugriff verweigert: Sie haben keine Berechtigung.";
                        break;
                    case StatusCodes.Status403Forbidden:
                        message = "Zugriff verweigert: Sie haben keine Berechtigung.";
                        break;
                    case StatusCodes.Status404NotFound:
                        message = "Die angeforderte Ressource wurde nicht gefunden.";
                        break;
                    case StatusCodes.Status500InternalServerError:
                        message = "Ein interner Serverfehler ist aufgetreten.";
                        break;
                }

                if (!string.IsNullOrEmpty(message))
                {
                    await context.Response.WriteAsJsonAsync(new
                    {
                        Message = message,
                        StatusCode = statusCode
                    });
                }
            }
        }
    }

}
