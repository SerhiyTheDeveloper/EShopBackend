using MINT.EShop.API.Wrappers;
using Web.Services;

namespace Web.Extensions
{
    /// <summary>
    /// Спільна обгортка fetch-endpoints Web, що викликають API від імені користувача:
    /// спершу оновлює токен (це справжній HTTP-запит, cookie записати можна), потім викликає API.
    /// </summary>
    public static class AuthorizedCall
    {
        public static async Task<IResult> RunAsync<T>(HttpContext context, TokenRefreshService refresher,
            Func<string, Task<APIResponse<T>>> call)
        {
            var state = await refresher.EnsureFreshAsync(context);

            if (state.Unavailable)
            {
                return Results.Json(
                    APIResponse<T>.FailureResponse("Не вдалося зв'язатися з сервером. Спробуйте пізніше."),
                    statusCode: StatusCodes.Status503ServiceUnavailable);
            }

            if (state.AccessToken is null)
            {
                return Results.Json(
                    APIResponse<T>.FailureResponse(ApiClientBase.SessionExpiredMessage),
                    statusCode: StatusCodes.Status401Unauthorized);
            }

            var response = await call(state.AccessToken);

            var statusCode = response.Success ? StatusCodes.Status200OK
                : response.Message == ApiClientBase.SessionExpiredMessage ? StatusCodes.Status401Unauthorized
                : StatusCodes.Status400BadRequest;

            return Results.Json(response, statusCode: statusCode);
        }
    }
}
