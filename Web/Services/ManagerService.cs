using Microsoft.JSInterop;
using MINT.EShop.API.Wrappers;
using MINT.EShop.Business.DTOs.Category;
using MINT.EShop.Business.DTOs.Producers;
using MINT.EShop.Business.DTOs.Products;

namespace Web.Services
{
    /// <summary>
    /// Дані сторінки менеджера. Читання каталогу публічне і йде напряму до API. Запис іде через fetch
    /// до Web (/api/manager/...), де токен оновлюється непомітно: circuit не може записати cookie,
    /// а його копія токена з часом застаріває.
    /// </summary>
    public class ManagerService(IJSRuntime js, CatalogApiClient catalog)
    {
        private readonly IJSRuntime _js = js;
        private readonly CatalogApiClient _catalog = catalog;

        public Task<APIResponse<List<CategoryResponse>>> GetCategoriesAsync() => _catalog.GetCategoriesAsync();

        public Task<APIResponse<List<ProducerResponse>>> GetProducersAsync() => _catalog.GetProducersAsync();

        public Task<APIResponse<List<ProductResponse>>> GetProductsAsync() => _catalog.GetProductsAsync();

        // Усі методи запису повертають текст помилки або null при успіху

        public Task<string?> CreateCategoryAsync(CreateCategoryRequest request)
            => WriteAsync("POST", "categories", request);

        public Task<string?> UpdateCategoryAsync(Guid id, UpdateCategoryRequest request)
            => WriteAsync("PUT", $"categories/{id}", request);

        public Task<string?> DeleteCategoryAsync(Guid id)
            => WriteAsync("DELETE", $"categories/{id}", null);

        public Task<string?> CreateProducerAsync(CreateProducerRequest request)
            => WriteAsync("POST", "producers", request);

        public Task<string?> UpdateProducerAsync(Guid id, UpdateProducerRequest request)
            => WriteAsync("PUT", $"producers/{id}", request);

        public Task<string?> DeleteProducerAsync(Guid id)
            => WriteAsync("DELETE", $"producers/{id}", null);

        public Task<string?> CreateProductAsync(CreateProductRequest request)
            => WriteAsync("POST", "products", request);

        public Task<string?> UpdateProductAsync(Guid id, UpdateProductRequest request)
            => WriteAsync("PUT", $"products/{id}", request);

        public Task<string?> DeleteProductAsync(Guid id)
            => WriteAsync("DELETE", $"products/{id}", null);

        private async Task<string?> WriteAsync(string method, string path, object? body)
        {
            try
            {
                var response = await _js.InvokeAsync<APIResponse<object>>("callManagerApi", method, path, body);
                if (response is { Success: true }) return null;

                return string.IsNullOrWhiteSpace(response?.Message)
                    ? "Не вдалося виконати дію. Спробуйте ще раз."
                    : response.Message;
            }
            catch (InvalidOperationException)
            {
                // Prerender: JS ще недоступний
                return "Інтерфейс ще завантажується. Спробуйте ще раз.";
            }
            catch (Exception)
            {
                return "Помилка з'єднання. Спробуйте ще раз.";
            }
        }
    }
}
