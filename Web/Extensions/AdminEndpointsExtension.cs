using Web.Services;

namespace Web.Extensions
{
    /// <summary>
    /// Endpoints адміна для виклику з браузера через fetch. Права перевіряє API (AdminPolicy):
    /// без ролі Admin він поверне 403.
    /// </summary>
    public static class AdminEndpointsExtension
    {
        public static WebApplication MapAdminEndpoints(this WebApplication app)
        {
            app.MapGet("/api/admin/users", (HttpContext context, TokenRefreshService refresher, AdminApiClient apiClient) =>
                AuthorizedCall.RunAsync(context, refresher, token => apiClient.GetUsersAsync(token)));

            app.MapPost("/api/admin/users/{id:guid}/promote", (Guid id, HttpContext context,
                TokenRefreshService refresher, AdminApiClient apiClient) =>
                AuthorizedCall.RunAsync(context, refresher, token => apiClient.PromoteToManagerAsync(token, id)));

            app.MapPost("/api/admin/users/{id:guid}/demote", (Guid id, HttpContext context,
                TokenRefreshService refresher, AdminApiClient apiClient) =>
                AuthorizedCall.RunAsync(context, refresher, token => apiClient.DemoteToClientAsync(token, id)));

            app.MapDelete("/api/admin/users/{id:guid}", (Guid id, HttpContext context,
                TokenRefreshService refresher, AdminApiClient apiClient) =>
                AuthorizedCall.RunAsync(context, refresher, token => apiClient.DeleteUserAsync(token, id)));

            return app;
        }
    }
}
