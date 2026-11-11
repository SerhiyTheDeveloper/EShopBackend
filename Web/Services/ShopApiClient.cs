using MINT.EShop.API.Wrappers;
using MINT.EShop.Business.DTOs.Orders;
using Web.Models;

namespace Web.Services
{
    /// <summary>
    /// Дії покупця від імені авторизованого користувача: оформлення замовлення.
    /// </summary>
    public class ShopApiClient(HttpClient httpClient) : ApiClientBase(httpClient)
    {
        public Task<APIResponse<OrderCreatedResponse>> CreateOrderAsync(string accessToken, CreateOrderRequest request, CancellationToken cancToken = default)
            => SendAsync<OrderCreatedResponse>(HttpMethod.Post, "api/v1/orders", accessToken, request, cancToken);
    }
}
