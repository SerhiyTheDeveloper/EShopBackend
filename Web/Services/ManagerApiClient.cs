using MINT.EShop.API.Wrappers;
using MINT.EShop.Business.DTOs.Category;
using MINT.EShop.Business.DTOs.Producers;
using MINT.EShop.Business.DTOs.Products;

namespace Web.Services
{
    /// <summary>
    /// Запис каталогу від імені менеджера: створення та видалення категорій, виробників і товарів,
    /// зміна товарів. Права перевіряє API (ManagerPolicy).
    /// </summary>
    public class ManagerApiClient(HttpClient httpClient) : ApiClientBase(httpClient)
    {
        public Task<APIResponse<CategoryResponse>> CreateCategoryAsync(string accessToken, CreateCategoryRequest request, CancellationToken cancToken = default)
            => SendAsync<CategoryResponse>(HttpMethod.Post, "api/v1/categories", accessToken, request, cancToken);

        public Task<APIResponse<CategoryResponse>> UpdateCategoryAsync(string accessToken, Guid id, UpdateCategoryRequest request, CancellationToken cancToken = default)
            => SendAsync<CategoryResponse>(HttpMethod.Put, $"api/v1/categories/{id}", accessToken, request, cancToken);

        public Task<APIResponse<object>> DeleteCategoryAsync(string accessToken, Guid id, CancellationToken cancToken = default)
            => SendAsync<object>(HttpMethod.Delete, $"api/v1/categories/{id}", accessToken, null, cancToken);

        public Task<APIResponse<ProducerResponse>> CreateProducerAsync(string accessToken, CreateProducerRequest request, CancellationToken cancToken = default)
            => SendAsync<ProducerResponse>(HttpMethod.Post, "api/v1/producers", accessToken, request, cancToken);

        public Task<APIResponse<ProducerResponse>> UpdateProducerAsync(string accessToken, Guid id, UpdateProducerRequest request, CancellationToken cancToken = default)
            => SendAsync<ProducerResponse>(HttpMethod.Put, $"api/v1/producers/{id}", accessToken, request, cancToken);

        public Task<APIResponse<object>> DeleteProducerAsync(string accessToken, Guid id, CancellationToken cancToken = default)
            => SendAsync<object>(HttpMethod.Delete, $"api/v1/producers/{id}", accessToken, null, cancToken);

        public Task<APIResponse<ProductResponse>> CreateProductAsync(string accessToken, CreateProductRequest request, CancellationToken cancToken = default)
            => SendAsync<ProductResponse>(HttpMethod.Post, "api/v1/products", accessToken, request, cancToken);

        public Task<APIResponse<ProductResponse>> UpdateProductAsync(string accessToken, Guid id, UpdateProductRequest request, CancellationToken cancToken = default)
            => SendAsync<ProductResponse>(HttpMethod.Put, $"api/v1/products/{id}", accessToken, request, cancToken);

        public Task<APIResponse<object>> DeleteProductAsync(string accessToken, Guid id, CancellationToken cancToken = default)
            => SendAsync<object>(HttpMethod.Delete, $"api/v1/products/{id}", accessToken, null, cancToken);
    }
}
