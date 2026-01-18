namespace BarberShopAPI.Helpers
{
    using global::BarberShopAPI.Exceptions;
    using Serilog;
    using System.Net;
    using System.Text.Json;

    namespace BarberShopAPI.Middlewares
    {
        public class ExceptionHandlingMiddleware
        {
            private readonly RequestDelegate _next;

            public ExceptionHandlingMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task Invoke(HttpContext context)
            {
                try
                {
                    await _next(context);
                }
                catch (BaseException ex)
                {
                    Log.Warning(ex, "Handled domain exception: {Code}", ex.Code);
                    await HandleBaseException(context, ex);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Unhandled exception occurred");
                    await HandleGenericException(context);
                }
            }

            private static async Task HandleBaseException(HttpContext context, BaseException ex)
            {
                context.Response.ContentType = "application/json";

                context.Response.StatusCode = ex switch
                {
                    NotFoundException => (int)HttpStatusCode.NotFound,
                    BadRequestException => (int)HttpStatusCode.BadRequest,
                    ConflictException => (int)HttpStatusCode.Conflict,
                    _ => (int)HttpStatusCode.InternalServerError
                };

                var payload = new
                {
                    code = ex.Code,
                    message = ex.Message
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
            }

            private static async Task HandleGenericException(HttpContext context)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var payload = new
                {
                    code = "INTERNAL_SERVER_ERROR",
                    message = "An unexpected error occurred."
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
            }
        }
    }
}
