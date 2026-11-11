using Microsoft.JSInterop;
using MINT.EShop.API.Wrappers;
using MINT.EShop.Business.DTOs.Identity;

namespace Web.Services
{
    /// <summary>
    /// Стан поточного користувача в межах circuit. Один екземпляр спільний для
    /// SettingsDrawer (ім'я та ініціали) і сторінки налаштувань; після збереження
    /// обидва оновлюються через подію OnChange.
    /// </summary>
    /// <remarks>
    /// Circuit не може записати cookie, а його копія токена з часом застаріває, тому
    /// збереження завжди йде через fetch до Web (/api/user/me), де токени оновлюються непомітно.
    /// Первинне завантаження (cookie вже свіжі завдяки middleware) йде напряму і лише при
    /// простроченому токені переходить на fetch.
    /// </remarks>
    public class UserProfileService(UserApiClient api, TokenStorageService tokenStorage, IJSRuntime js)
    {
        private readonly UserApiClient _api = api;
        private readonly TokenStorageService _tokenStorage = tokenStorage;
        private readonly IJSRuntime _js = js;
        private Task<string?>? _loadTask;

        public UserResponse? User { get; private set; }
        public event Action? OnChange;

        public string DisplayName
        {
            get
            {
                var name = $"{User?.FirstName} {User?.LastName}".Trim();
                return name.Length == 0 ? "Користувач" : name;
            }
        }

        public string Initials
        {
            get
            {
                var initials = FirstLetter(User?.FirstName) + FirstLetter(User?.LastName);
                return initials.Length == 0 ? "?" : initials;
            }
        }

        /// <summary>
        /// Завантажує дані користувача з API. Повертає текст помилки або null при успіху
        /// (або якщо користувач не авторизований). Паралельні виклики ділять один запит.
        /// </summary>
        public Task<string?> LoadAsync(bool force = false)
        {
            if (User is not null && !force) return Task.FromResult<string?>(null);
            if (_loadTask is { IsCompleted: false }) return _loadTask;

            return _loadTask = FetchAsync();
        }

        /// <summary>
        /// Зберігає ім'я, прізвище та телефон. Повертає текст помилки або null при успіху.
        /// </summary>
        public async Task<string?> UpdateAsync(UpdateUserDataRequest request)
        {
            var response = await CallViaBrowserAsync("updateCurrentUser", request);
            if (!response.Success || response.Data is null)
            {
                return string.IsNullOrWhiteSpace(response.Message)
                    ? "Не вдалося зберегти зміни. Спробуйте ще раз."
                    : response.Message;
            }

            User = response.Data;
            OnChange?.Invoke();
            return null;
        }

        private async Task<string?> FetchAsync()
        {
            // Гість: немає access token, API не викликаємо
            var accessToken = _tokenStorage.GetAccessToken();
            if (string.IsNullOrEmpty(accessToken)) return null;

            var response = TokenStorageService.IsExpired(accessToken)
                ? await CallViaBrowserAsync("getCurrentUser")
                : await _api.GetCurrentAsync(accessToken);

            // Токен circuit міг застаріти: пробуємо ще раз через fetch, де його оновлять
            if (response.Message == UserApiClient.SessionExpiredMessage)
            {
                response = await CallViaBrowserAsync("getCurrentUser");
            }

            if (!response.Success || response.Data is null)
            {
                return string.IsNullOrWhiteSpace(response.Message)
                    ? "Не вдалося завантажити дані акаунта."
                    : response.Message;
            }

            User = response.Data;
            OnChange?.Invoke();
            return null;
        }

        private async Task<APIResponse<UserResponse>> CallViaBrowserAsync(string function, params object?[] args)
        {
            try
            {
                var result = await _js.InvokeAsync<APIResponse<UserResponse>>(function, args);
                return result ?? APIResponse<UserResponse>.FailureResponse("Помилка обробки відповіді сервера.");
            }
            catch (InvalidOperationException)
            {
                // Prerender: JS ще недоступний, дані завантажаться в circuit
                return APIResponse<UserResponse>.FailureResponse(UserApiClient.SessionExpiredMessage);
            }
            catch (Exception)
            {
                return APIResponse<UserResponse>.FailureResponse("Помилка з'єднання. Спробуйте ще раз.");
            }
        }

        private static string FirstLetter(string? value)
            => string.IsNullOrWhiteSpace(value) ? string.Empty : char.ToUpperInvariant(value.Trim()[0]).ToString();
    }
}
