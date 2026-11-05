using MINT.EShop.Business.DTOs.Identity;
using System.Net;
using System.Net.Http.Headers;
using Web.Services;

namespace Web
{
    public class AuthHttpMessageHandler(IHttpClientFactory httpClientFactory,
        TokenStorageService tokenStorage, CookieAuthenticationStateProvider authStateProvider)
        : DelegatingHandler
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly TokenStorageService _tokenStorage = tokenStorage;
        private readonly CookieAuthenticationStateProvider _authStateProvider = authStateProvider;
        private readonly SemaphoreSlim _refreshLock = new(1, 1);

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = _tokenStorage.GetAccessToken();

            if (!string.IsNullOrEmpty(accessToken))
            {
                accessToken = await RefreshTokenAsync(cancellationToken);
            }

            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newToken = await RefreshTokenAsync(cancellationToken);

                if (!string.IsNullOrEmpty(newToken))
                {
                    var retryRequest = await CloneRequestAsync(request);
                    retryRequest.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", newToken);

                    response = await base.SendAsync(retryRequest, cancellationToken);
                }
            }

            return response;
        }

        private async Task<string?> RefreshTokenAsync(CancellationToken cancellationToken)
        {
            await _refreshLock.WaitAsync(cancellationToken);
            try
            {
                var refreshToken = _tokenStorage.GetRefreshToken();
                if (string.IsNullOrEmpty(refreshToken)) return null;

                var client = _httpClientFactory.CreateClient("NoAuth");
                var response = await client.PostAsJsonAsync(
                    "/api/auth/refresh",
                    new { RefreshToken = refreshToken },
                    cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    _tokenStorage.ClearTokens();
                    _authStateProvider.NotifyUserLoggedOut();
                    return null;
                }

                var result = await response.Content
                    .ReadFromJsonAsync<LoginResponse>(cancellationToken: cancellationToken);

                if (result is null) return null;

                _tokenStorage.SetTokens(result.AccessToken, result.RefreshToken);
                _authStateProvider.NotifyUserLoggedIn(result.AccessToken);

                return result.AccessToken;
            }
            finally
            {
                _refreshLock.Release();
            }
        }

        private static async Task<HttpRequestMessage> CloneRequestAsync(HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri);

            if (request.Content is not null)
            {
                var content = await request.Content.ReadAsByteArrayAsync();
                clone.Content = new ByteArrayContent(content);
                foreach (var header in request.Content.Headers)
                    clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            foreach (var header in request.Headers)
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

            return clone;
        }
    }
}
