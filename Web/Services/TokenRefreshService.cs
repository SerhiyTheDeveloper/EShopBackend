using Microsoft.Extensions.Options;
using MINT.EShop.Business.DTOs.Identity;
using System.Collections.Concurrent;
using Web.Extensions;

namespace Web.Services
{
    /// <summary>
    /// Результат перевірки токена: актуальний access-токен (або null, якщо сесії немає)
    /// і прапорець, що оновити токен не вдалося через недоступність API.
    /// </summary>
    public record TokenState(string? AccessToken, bool Unavailable = false);

    /// <summary>
    /// Оновлює токени у справжньому HTTP-запиті (middleware сторінок і fetch-endpoints),
    /// де можна записати cookie. З Blazor-circuit цього робити не можна.
    /// </summary>
    public class TokenRefreshService(AuthApiClient apiClient, IOptions<TokenOptions> tokenOptions)
    {
        // API при refresh міняє refresh-токен, тому паралельні запити з тим самим токеном
        // мають ділити один виклик, інакше другий отримає "Invalid refresh token".
        private static readonly ConcurrentDictionary<string, Lazy<Task<RefreshOutcome>>> InFlight = new();
        private static readonly TimeSpan Reserve = TimeSpan.FromMinutes(1);
        private static readonly TimeSpan KeepResult = TimeSpan.FromSeconds(30);

        public async Task<TokenState> EnsureFreshAsync(HttpContext context)
        {
            // У цьому запиті токени вже оновлено
            if (context.Items[TokenStorageService.ItemsKey] is TokenPair done)
            {
                return new TokenState(done.AccessToken);
            }

            var options = tokenOptions.Value;
            var accessToken = context.Request.Cookies[options.AccessToken];
            var refreshToken = context.Request.Cookies[options.RefreshToken];

            // API вимагає і access, і refresh-токен, без одного з них оновити неможливо
            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            {
                return new TokenState(null);
            }

            if (!TokenStorageService.IsExpired(accessToken, Reserve))
            {
                return new TokenState(accessToken);
            }

            var lazy = InFlight.GetOrAdd(refreshToken, _ => new Lazy<Task<RefreshOutcome>>(() =>
                apiClient.RefreshAsync(new RefreshRequest
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                })));

            var outcome = await lazy.Value;
            _ = Task.Delay(KeepResult).ContinueWith(_ => InFlight.TryRemove(refreshToken, out var removed));

            if (outcome.Tokens is not null)
            {
                AuthCookies.Set(context.Response, options, outcome.Tokens);
                context.Items[TokenStorageService.ItemsKey] =
                    new TokenPair(outcome.Tokens.AccessToken, outcome.Tokens.RefreshToken);
                return new TokenState(outcome.Tokens.AccessToken);
            }

            if (outcome.Rejected)
            {
                // Refresh-токен недійсний або протермінований: сесії більше немає
                AuthCookies.Delete(context.Response, options);
                context.Items[TokenStorageService.ItemsKey] = new TokenPair(null, null);
                return new TokenState(null);
            }

            // API недоступний: cookie не чіпаємо, спробуємо знову наступного разу
            return new TokenState(null, Unavailable: true);
        }
    }
}
