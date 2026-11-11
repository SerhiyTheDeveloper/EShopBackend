using Web.Services;

namespace Web.Middleware
{
    /// <summary>
    /// Оновлює токени при відкритті сторінки, щоб prerender і новий circuit
    /// одразу отримали свіжі cookie. Саму логіку refresh виконує TokenRefreshService.
    /// </summary>
    public class TokenRefreshMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context, TokenRefreshService refresher)
        {
            if (IsPageRequest(context.Request))
            {
                await refresher.EnsureFreshAsync(context);
            }

            await next(context);
        }

        private static bool IsPageRequest(HttpRequest request)
        {
            return HttpMethods.IsGet(request.Method)
                && request.Headers.Accept.ToString().Contains("text/html", StringComparison.OrdinalIgnoreCase);
        }
    }
}
