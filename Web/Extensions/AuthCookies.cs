using MINT.EShop.Business.DTOs.Identity;

namespace Web.Extensions
{
    /// <summary>
    /// Єдине місце запису auth-cookie. Обидві cookie живуть до ExpiresDate refresh-токена:
    /// JWT усередині access-cookie дійсний недовго (ExpiresInMinutes), але протермінований
    /// токен потрібен API разом з refresh-токеном, тому cookie не має зникати разом з ним.
    /// </summary>
    public static class AuthCookies
    {
        public static void Set(HttpResponse response, TokenOptions options, LoginResponse tokens)
        {
            var expires = tokens.ExpiresDate.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(tokens.ExpiresDate, DateTimeKind.Utc)
                : tokens.ExpiresDate;

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = expires
            };

            response.Cookies.Append(options.AccessToken, tokens.AccessToken, cookieOptions);
            response.Cookies.Append(options.RefreshToken, tokens.RefreshToken, cookieOptions);
        }

        public static void Delete(HttpResponse response, TokenOptions options)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            response.Cookies.Delete(options.AccessToken, cookieOptions);
            response.Cookies.Delete(options.RefreshToken, cookieOptions);
        }
    }
}
