using Microsoft.Extensions.Options;
using MINT.EShop.Business.DTOs.Identity;
using Web.Services;

namespace Web.Extensions
{
    public static class AuthEndpointsExtension
    {
        public static WebApplication MapAuthEndpoints(this WebApplication app)
        {
            app.MapPost("/api/auth/login", LoginAsync);
            app.MapPost("/api/auth/register", RegisterAsync);
            app.MapPost("/api/auth/verify", VerifyAsync);
            app.MapPost("/api/auth/resent", ResentAsync);
            app.MapPost("/api/auth/logout", LogoutAsync);

            return app;
        }

        private static async Task<IResult> LogoutAsync(
            HttpContext httpContext, AuthApiClient apiClient,
            IOptions<TokenOptions> tokenOptions)
        {
            var refreshToken = httpContext.Request.Cookies[tokenOptions.Value.RefreshToken];
            if (!string.IsNullOrEmpty(refreshToken))
            {
                try
                {
                    await apiClient.LogoutAsync(new LogoutRequest { RefreshToken = refreshToken });
                }
                catch
                {
                }
            }

            AuthCookies.Delete(httpContext.Response, tokenOptions.Value);

            return Results.Ok();
        }

        private static async Task<IResult> LoginAsync(
            LoginRequest request, AuthApiClient apiClient,
            HttpContext httpContext, IOptions<TokenOptions> tokenOptions)
        {
            string? errorMessage = null;

            try
            {
                var response = await apiClient.LoginAsync(request);

                if (response.Success)
                {
                    if (response.Data == null)
                    {
                        errorMessage = "Помилка сервера. Спробуйте ще раз.";
                    }
                    else
                    {
                        AuthCookies.Set(httpContext.Response, tokenOptions.Value, response.Data);
                    }
                }
                else
                {
                    errorMessage = response.Message;
                }

                return Results.Ok(errorMessage);
            }
            catch
            {
                errorMessage = "Помилка з'єднання. Спробуйте ще раз.";
                return Results.Ok(errorMessage);
            }
        }

        private static async Task<IResult> RegisterAsync(
            RegisterRequest request, AuthApiClient apiClient)
        {
            string? errorMessage = null;

            try
            {
                var response = await apiClient.RegisterAsync(request);

                if (response.Success)
                {
                    if (response.Data == null)
                    {
                        errorMessage = "Помилка сервера. Спробуйте ще раз.";
                    }
                }
                else
                {
                    errorMessage = response.Message;
                }

                return Results.Ok(errorMessage);
            }
            catch
            {
                errorMessage = "Помилка з'єднання. Спробуйте ще раз.";
                return Results.Ok(errorMessage);
            }
        }

        private static async Task<IResult> ResentAsync(
            ResentRequest request, AuthApiClient apiClient)
        {
            string? errorMessage = null;

            try
            {
                var response = await apiClient.ResentAsync(request);

                if (response.Success)
                {
                    if (response.Data == null)
                    {
                        errorMessage = "Помилка сервера. Спробуйте ще раз.";
                    }
                }
                else
                {
                    errorMessage = response.Message;
                }

                return Results.Ok(errorMessage);
            }
            catch
            {
                errorMessage = "Помилка з'єднання. Спробуйте ще раз.";
                return Results.Ok(errorMessage);
            }
        }

        private static async Task<IResult> VerifyAsync(
            VerifyRequest request, AuthApiClient apiClient,
            HttpContext httpContext, IOptions<TokenOptions> tokenOptions)
        {
            string? errorMessage = null;
            try
            {
                var response = await apiClient.VerifyAsync(request);
                if (response.Success)
                {
                    if (response.Data == null)
                    {
                        errorMessage = "Помилка сервера. Спробуйте ще раз.";
                    }
                    else
                    {
                        AuthCookies.Set(httpContext.Response, tokenOptions.Value, response.Data.LoginResponse);
                    }
                }
                else
                {
                    errorMessage = response.Message;
                }
                return Results.Ok(errorMessage);
            }
            catch
            {
                errorMessage = "Помилка з'єднання. Спробуйте ще раз.";
                return Results.Ok(errorMessage);
            }
        }
    }
}