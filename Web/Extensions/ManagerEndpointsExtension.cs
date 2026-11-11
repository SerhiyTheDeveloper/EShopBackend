using MINT.EShop.Business.DTOs.Category;
using MINT.EShop.Business.DTOs.Producers;
using MINT.EShop.Business.DTOs.Products;
using Web.Services;

namespace Web.Extensions
{
    /// <summary>
    /// Endpoints запису каталогу для виклику з браузера через fetch: токен оновлюється непомітно
    /// (це справжній HTTP-запит), а права перевіряє API (ManagerPolicy).
    /// </summary>
    public static class ManagerEndpointsExtension
    {
        public static WebApplication MapManagerEndpoints(this WebApplication app)
        {
            app.MapPost("/api/manager/categories", (CreateCategoryRequest request, HttpContext context,
                TokenRefreshService refresher, ManagerApiClient apiClient) =>
                AuthorizedCall.RunAsync(context, refresher, token => apiClient.CreateCategoryAsync(token, request)));

            app.MapPut("/api/manager/categories/{id:guid}", (Guid id, UpdateCategoryRequest request, HttpContext context,
                TokenRefreshService refresher, ManagerApiClient apiClient) =>
                AuthorizedCall.RunAsync(context, refresher, token => apiClient.UpdateCategoryAsync(token, id, request)));

            app.MapDelete("/api/manager/categories/{id:guid}", (Guid id, HttpContext context,
                TokenRefreshService refresher, ManagerApiClient apiClient) =>
                AuthorizedCall.RunAsync(context, refresher, token => apiClient.DeleteCategoryAsync(token, id)));

            app.MapPost("/api/manager/producers", (CreateProducerRequest request, HttpContext context,
                TokenRefreshService refresher, ManagerApiClient apiClient) =>
                AuthorizedCall.RunAsync(context, refresher, token => apiClient.CreateProducerAsync(token, request)));

            app.MapPut("/api/manager/producers/{id:guid}", (Guid id, UpdateProducerRequest request, HttpContext context,
                TokenRefreshService refresher, ManagerApiClient apiClient) =>
                AuthorizedCall.RunAsync(context, refresher, token => apiClient.UpdateProducerAsync(token, id, request)));

            app.MapDelete("/api/manager/producers/{id:guid}", (Guid id, HttpContext context,
                TokenRefreshService refresher, ManagerApiClient apiClient) =>
                AuthorizedCall.RunAsync(context, refresher, token => apiClient.DeleteProducerAsync(token, id)));

            app.MapPost("/api/manager/products", (CreateProductRequest request, HttpContext context,
                TokenRefreshService refresher, ManagerApiClient apiClient) =>
                AuthorizedCall.RunAsync(context, refresher, token => apiClient.CreateProductAsync(token, request)));

            app.MapPut("/api/manager/products/{id:guid}", (Guid id, UpdateProductRequest request, HttpContext context,
                TokenRefreshService refresher, ManagerApiClient apiClient) =>
                AuthorizedCall.RunAsync(context, refresher, token => apiClient.UpdateProductAsync(token, id, request)));

            app.MapDelete("/api/manager/products/{id:guid}", (Guid id, HttpContext context,
                TokenRefreshService refresher, ManagerApiClient apiClient) =>
                AuthorizedCall.RunAsync(context, refresher, token => apiClient.DeleteProductAsync(token, id)));

            return app;
        }
    }
}
