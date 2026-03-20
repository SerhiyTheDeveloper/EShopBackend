using MINT.EShop.API.Wrappers;
using System.ComponentModel;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MINT.EShop.API.Middlewares
{
    public class GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger, IHostEnvironment env)
    {
        // TODO: Обробити можливу помилку DbUpdateException пов'язану з випадковим записом однакових RefreshToken у UserSession.(?)
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            var statusCode = ex switch
            {
                KeyNotFoundException => HttpStatusCode.NotFound,
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                ArgumentException or InvalidOperationException => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = (int)statusCode;

            var errorList = new List<string> { ex.GetType().Name };

            if (env.IsDevelopment())
            {
                errorList.Add(ex.StackTrace ?? string.Empty);
            }

            string message = (statusCode == HttpStatusCode.InternalServerError && !env.IsDevelopment())
                ? "An unexpected error occurred. Please try again later." 
                : ex.Message;

            var response = APIResponse.FailureResponse(message, errorList);

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            var jsonResponse = JsonSerializer.Serialize(response, jsonOptions);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
