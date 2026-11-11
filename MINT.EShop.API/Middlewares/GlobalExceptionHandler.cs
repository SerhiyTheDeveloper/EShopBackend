using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MINT.EShop.API.Wrappers;
using System.Net;
using Npgsql;
using System.Text.Json;

namespace MINT.EShop.API.Middlewares
{
    public class GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger, IHostEnvironment env)
    {
        // TODO: Обробити можливу помилку DbUpdateException пов'язану з випадковим записом однакових RefreshToken у UserSession. (?)
        // TODO: Обробити можливу помилку DbUpdateException пов'язану з одночасною купівлею двох продуктів. (?)
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

        private static (HttpStatusCode Status, string Message, string Code)? MapDatabaseError(Exception ex)
        {
            if (ex is not DbUpdateException { InnerException: PostgresException postgresError }) return null;

            return postgresError.SqlState switch
            {
                PostgresErrorCodes.UniqueViolation =>
                    (HttpStatusCode.Conflict, "A record with the same unique value already exists.", "unique_violation"),
                PostgresErrorCodes.ForeignKeyViolation =>
                    (HttpStatusCode.Conflict, "The operation conflicts with related records.", "foreign_key_violation"),
                PostgresErrorCodes.CheckViolation =>
                    (HttpStatusCode.BadRequest, "The value violates a database constraint.", "check_violation"),
                _ => null
            };
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            var statusCode = ex switch
            {
                KeyNotFoundException => HttpStatusCode.NotFound,
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                ArgumentException or InvalidOperationException => HttpStatusCode.BadRequest,
                SecurityTokenException => HttpStatusCode.Unauthorized,
                _ => HttpStatusCode.InternalServerError
            };

            // Помилки бази даних (дублікат, пов'язані записи) віддаємо як 409/400 з кодом замість 500
            var dbError = MapDatabaseError(ex);
            if (dbError is not null) statusCode = dbError.Value.Status;

            context.Response.StatusCode = (int)statusCode;

            var errorList = new List<string> { dbError?.Code ?? ex.GetType().Name };

            if (env.IsDevelopment())
            {
                errorList.Add(ex.StackTrace ?? string.Empty);
            }

            string message = dbError?.Message ?? ((statusCode == HttpStatusCode.InternalServerError && !env.IsDevelopment())
                ? "An unexpected error occurred. Please try again later." 
                : ex.Message);

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
