using MINT.EShop.Business.DTOs.Orders;
using Web.Services;

namespace Web.Extensions
{
    /// <summary>
    /// Endpoints покупця для виклику з браузера через fetch: токен оновлюється непомітно
    /// (це справжній HTTP-запит), а права перевіряє API ([Authorize] на контролері замовлень).
    /// </summary>
    public static class ShopEndpointsExtension
    {
        public static WebApplication MapShopEndpoints(this WebApplication app)
        {
            app.MapPost("/api/shop/orders", (CreateOrderRequest request, HttpContext context,
                TokenRefreshService refresher, ShopApiClient apiClient) =>
                AuthorizedCall.RunAsync(context, refresher, token => apiClient.CreateOrderAsync(token, request)));

            app.MapGet("/api/shop/orders/my", (HttpContext context, TokenRefreshService refresher, ShopApiClient apiClient) =>
                AuthorizedCall.RunAsync(context, refresher, token => apiClient.GetMyOrdersAsync(token)));

            app.MapPut("/api/shop/orders/my/{id:guid}/cancel", (Guid id, HttpContext context,
                TokenRefreshService refresher, ShopApiClient apiClient) =>
                AuthorizedCall.RunAsync(context, refresher, token => apiClient.CancelMyOrderAsync(token, id)));

            return app;
        }
    }
}
