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

            return app;
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
                        httpContext.Response.Cookies.Append(tokenOptions.Value.AccessToken, response.Data.AccessToken, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict,
                            Expires = DateTimeOffset.UtcNow.AddMinutes(tokenOptions.Value.ExpiresInMinutes)
                        });
                        httpContext.Response.Cookies.Append(tokenOptions.Value.RefreshToken, response.Data.RefreshToken, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict,
                            Expires = response.Data.ExpiresDate
                        });
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
                        httpContext.Response.Cookies.Append(tokenOptions.Value.AccessToken, response.Data.LoginResponse.AccessToken, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict,
                            Expires = DateTimeOffset.UtcNow.AddMinutes(tokenOptions.Value.ExpiresInMinutes)
                        });
                        httpContext.Response.Cookies.Append(tokenOptions.Value.RefreshToken, response.Data.LoginResponse.RefreshToken, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict,
                            Expires = response.Data.LoginResponse.ExpiresDate
                        });
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