using MINT.EShop.API.Wrappers;
using MINT.EShop.Business.DTOs.Orders;
using Web.Models;

namespace Web.Services
{
    /// <summary>
    /// Дії покупця від імені авторизованого користувача: оформлення, перегляд і скасування замовлень.
    /// </summary>
    public class ShopApiClient(HttpClient httpClient) : ApiClientBase(httpClient)
    {
        public Task<APIResponse<OrderCreatedResponse>> CreateOrderAsync(string accessToken, CreateOrderRequest request, CancellationToken cancToken = default)
            => SendAsync<OrderCreatedResponse>(HttpMethod.Post, "api/v1/orders", accessToken, request, cancToken);

        public Task<APIResponse<List<OrderResponse>>> GetMyOrdersAsync(string accessToken, CancellationToken cancToken = default)
            => SendAsync<List<OrderResponse>>(HttpMethod.Get, "api/v1/orders/my", accessToken, null, cancToken);

        public Task<APIResponse<object>> CancelMyOrderAsync(string accessToken, Guid orderId, CancellationToken cancToken = default)
            => SendAsync<object>(HttpMethod.Put, $"api/v1/orders/my/{orderId}/cancel", accessToken, null, cancToken);
    }
}
