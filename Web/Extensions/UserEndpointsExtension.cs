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
                AuthorizedCall.RunAsync(context, refresher, token => apiClient.GetCurrentAsync(token)));

            app.MapPut("/api/user/me", (UpdateUserDataRequest request, HttpContext context,
                TokenRefreshService refresher, UserApiClient apiClient) =>
                AuthorizedCall.RunAsync(context, refresher, token => apiClient.UpdateCurrentAsync(token, request)));

            return app;
        }
    }
}
