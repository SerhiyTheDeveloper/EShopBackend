using MINT.EShop.API.Wrappers;
using MINT.EShop.Business.DTOs.Products;

namespace Web.Services
{
    /// <summary>
    /// Список вподобань поточного користувача (api/v1/wishlist).
    /// </summary>
    public class WishListApiClient(HttpClient httpClient) : ApiClientBase(httpClient)
    {
        public Task<APIResponse<List<ProductResponse>>> GetAllAsync(string accessToken, CancellationToken cancToken = default)
            => SendAsync<List<ProductResponse>>(HttpMethod.Get, "api/v1/wishlist", accessToken, null, cancToken);

        public Task<APIResponse<object>> AddAsync(string accessToken, Guid productId, CancellationToken cancToken = default)
            => SendAsync<object>(HttpMethod.Post, $"api/v1/wishlist/{productId}", accessToken, null, cancToken);

        public Task<APIResponse<object>> RemoveAsync(string accessToken, Guid productId, CancellationToken cancToken = default)
            => SendAsync<object>(HttpMethod.Delete, $"api/v1/wishlist/{productId}", accessToken, null, cancToken);
    }
}
