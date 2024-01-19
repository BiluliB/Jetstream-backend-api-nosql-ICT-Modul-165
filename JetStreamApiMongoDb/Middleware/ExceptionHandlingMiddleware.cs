using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System;
using System.Net;
using System.Threading.Tasks;

namespace JetStreamApiMongoDb.Middleware
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
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            string message = exception.Message;
            int statusCode = (int)HttpStatusCode.InternalServerError;

            if (exception is MongoCommandException mongoCommandException)
            {
                message = "Ein MongoDB-Fehler ist aufgetreten: " + mongoCommandException.Message;
                statusCode = (int)HttpStatusCode.BadRequest;
            }

            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsJsonAsync(new
            {
                Message = message,
                StatusCode = statusCode
            });
        }
    }
}
