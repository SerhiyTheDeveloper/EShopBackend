using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace Web.Services
{
    /// <summary>
    /// Пара токенів, які middleware щойно оновив у поточному запиті (cookie ще не повернулись
    /// від браузера, тому поточний запит бере їх звідси).
    /// </summary>
    public record TokenPair(string? AccessToken, string? RefreshToken);

    public class TokenStorageService(IOptions<TokenOptions> tokenOptions, IHttpContextAccessor httpContextAccessor)
    {
        public const string ItemsKey = "Web.TokenPair";

        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly TokenOptions _tokenOptions = tokenOptions.Value;

        public string? GetAccessToken()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.Items[ItemsKey] is TokenPair pair)
                return pair.AccessToken;

            return httpContext?.Request.Cookies[_tokenOptions.AccessToken];
        }

        public string? GetRefreshToken()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.Items[ItemsKey] is TokenPair pair)
                return pair.RefreshToken;

            return httpContext?.Request.Cookies[_tokenOptions.RefreshToken];
        }

        /// <summary>
        /// Чи прострочений JWT (з запасом skew). Непрочитний токен вважається простроченим.
        /// </summary>
        public static bool IsExpired(string jwt, TimeSpan? skew = null)
        {
            try
            {
                var token = new JwtSecurityTokenHandler().ReadJwtToken(jwt);
                return token.ValidTo <= DateTime.UtcNow.Add(skew ?? TimeSpan.Zero);
            }
            catch
            {
                return true;
            }
        }
    }
}
