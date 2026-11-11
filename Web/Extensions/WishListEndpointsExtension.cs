using Web.Services;

namespace Web.Extensions
{
    /// <summary>
    /// Endpoints вподобань для виклику з браузера через fetch: токен оновлюється непомітно
    /// (це справжній HTTP-запит), а права перевіряє API ([Authorize] на контролері вподобань).
    /// </summary>
    public static class WishListEndpointsExtension
    {
        public static WebApplication MapWishListEndpoints(this WebApplication app)
        {
            app.MapGet("/api/wishlist", (HttpContext context, TokenRefreshService refresher, WishListApiClient apiClient) =>
                AuthorizedCall.RunAsync(context, refresher, token => apiClient.GetAllAsync(token)));

            app.MapPost("/api/wishlist/{productId:guid}", (Guid productId, HttpContext context,
                TokenRefreshService refresher, WishListApiClient apiClient) =>
                AuthorizedCall.RunAsync(context, refresher, token => apiClient.AddAsync(token, productId)));

            app.MapDelete("/api/wishlist/{productId:guid}", (Guid productId, HttpContext context,
                TokenRefreshService refresher, WishListApiClient apiClient) =>
                AuthorizedCall.RunAsync(context, refresher, token => apiClient.RemoveAsync(token, productId)));

            return app;
        }
    }
}
