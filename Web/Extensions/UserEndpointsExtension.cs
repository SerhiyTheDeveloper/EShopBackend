using MINT.EShop.API.Wrappers;
using MINT.EShop.Business.DTOs.Identity;
using Web.Services;

namespace Web.Extensions
{
    /// <summary>
    /// Endpoints даних користувача для виклику з браузера через fetch. Це справжній HTTP-запит,
    /// тому тут можна непомітно оновити токени (і записати cookie) без перезавантаження сторінки.
    /// </summary>
    public static class UserEndpointsExtension
    {
        public static WebApplication MapUserEndpoints(this WebApplication app)
        {
            app.MapGet("/api/user/me", (HttpContext context, TokenRefreshService refresher, UserApiClient apiClient) =>
                CallAsync(context, refresher, token => apiClient.GetCurrentAsync(token)));

            app.MapPut("/api/user/me", (UpdateUserDataRequest request, HttpContext context,
                TokenRefreshService refresher, UserApiClient apiClient) =>
                CallAsync(context, refresher, token => apiClient.UpdateCurrentAsync(token, request)));

            return app;
        }

        private static async Task<IResult> CallAsync(HttpContext context, TokenRefreshService refresher,
            Func<string, Task<APIResponse<UserResponse>>> call)
        {
            var state = await refresher.EnsureFreshAsync(context);

            if (state.Unavailable)
            {
                return Results.Json(
                    APIResponse<UserResponse>.FailureResponse("Не вдалося зв'язатися з сервером. Спробуйте пізніше."),
                    statusCode: StatusCodes.Status503ServiceUnavailable);
            }

            if (state.AccessToken is null)
            {
                return Results.Json(
                    APIResponse<UserResponse>.FailureResponse(UserApiClient.SessionExpiredMessage),
                    statusCode: StatusCodes.Status401Unauthorized);
            }

            var response = await call(state.AccessToken);

            var statusCode = response.Success ? StatusCodes.Status200OK
                : response.Message == UserApiClient.SessionExpiredMessage ? StatusCodes.Status401Unauthorized
                : StatusCodes.Status400BadRequest;

            return Results.Json(response, statusCode: statusCode);
        }
    }
}
