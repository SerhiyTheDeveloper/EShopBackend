using Microsoft.Extensions.Options;

namespace Web.Services
{
    public class TokenStorageService(IOptions<TokenOptions> tokenOptions, IHttpContextAccessor httpContextAccessor)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly TokenOptions _tokenOptions = tokenOptions.Value;
        private string? accessToken;
        private string? refreshToken;

        public string? GetAccessToken()
        {
            if (!string.IsNullOrEmpty(accessToken)) 
                return accessToken;
            return _httpContextAccessor.HttpContext?
                .Request.Cookies[_tokenOptions.AccessToken];
        }

        public string? GetRefreshToken()
        {
            if (!string.IsNullOrEmpty(refreshToken)) 
                return refreshToken;
            return _httpContextAccessor.HttpContext?
                .Request.Cookies[_tokenOptions.RefreshToken];
        }

        public void ClearTokens()
        {
            accessToken = null;
            refreshToken = null;
        }

        public void SetTokens(string accessToken, string refreshToken)
        {
            this.accessToken = accessToken;
            this.refreshToken = refreshToken;
        }
    }
}
