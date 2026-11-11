using MINT.EShop.API.Wrappers;
using MINT.EShop.Business.DTOs.Category;
using MINT.EShop.Business.DTOs.Producers;
using MINT.EShop.Business.DTOs.Products;

namespace Web.Services
{
    /// <summary>
    /// Публічне читання каталогу (категорії, виробники, товари). Токен не потрібен.
    /// </summary>
    public class CatalogApiClient(HttpClient httpClient) : ApiClientBase(httpClient)
    {
        public Task<APIResponse<List<CategoryResponse>>> GetCategoriesAsync(CancellationToken cancToken = default)
            => SendAsync<List<CategoryResponse>>(HttpMethod.Get, "api/v1/categories", null, null, cancToken);

        public Task<APIResponse<List<ProducerResponse>>> GetProducersAsync(CancellationToken cancToken = default)
            => SendAsync<List<ProducerResponse>>(HttpMethod.Get, "api/v1/producers", null, null, cancToken);

        public Task<APIResponse<ProductResponse>> GetProductAsync(Guid id, CancellationToken cancToken = default)
            => SendAsync<ProductResponse>(HttpMethod.Get, $"api/v1/products/{id}", null, null, cancToken);

        public Task<APIResponse<List<ProductResponse>>> GetProductsByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancToken = default)
        {
            var query = string.Join("&", ids.Select(id => $"ids={id}"));
            return SendAsync<List<ProductResponse>>(HttpMethod.Get, $"api/v1/products/by-ids?{query}", null, null, cancToken);
        }

        public Task<APIResponse<List<ProductResponse>>> GetProductsAsync(CancellationToken cancToken = default)
            => SendAsync<List<ProductResponse>>(HttpMethod.Get, "api/v1/products", null, null, cancToken);
    }
}
