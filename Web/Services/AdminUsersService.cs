using Microsoft.JSInterop;
using MINT.EShop.API.Wrappers;
using MINT.EShop.Business.DTOs.Identity;

namespace Web.Services
{
    /// <summary>
    /// Дані адмін-сторінки. Усі виклики йдуть через fetch до Web (/api/admin/...), де токен
    /// оновлюється непомітно: circuit не може записати cookie, а його копія токена застаріває.
    /// </summary>
    public class AdminUsersService(IJSRuntime js)
    {
        private readonly IJSRuntime _js = js;

        public Task<APIResponse<List<UserResponse>>> GetUsersAsync()
            => CallAsync<List<UserResponse>>("getAdminUsers");

        /// <summary>
        /// Змінює роль: promote = true робить менеджером, false понижує до клієнта.
        /// Повертає текст помилки або null при успіху.
        /// </summary>
        public async Task<string?> ChangeRoleAsync(Guid userId, bool promote)
        {
            var response = await CallAsync<object>("changeUserRole", userId, promote);
            if (response.Success) return null;

            return string.IsNullOrWhiteSpace(response.Message)
                ? "Не вдалося змінити роль. Спробуйте ще раз."
                : response.Message;
        }

        /// <summary>
        /// Видаляє користувача. Повертає текст помилки або null при успіху.
        /// </summary>
        public async Task<string?> DeleteUserAsync(Guid userId)
        {
            var response = await CallAsync<object>("deleteUser", userId);
            if (response.Success) return null;

            return string.IsNullOrWhiteSpace(response.Message)
                ? "Не вдалося видалити користувача. Спробуйте ще раз."
                : response.Message;
        }

        private async Task<APIResponse<T>> CallAsync<T>(string function, params object?[] args)
        {
            try
            {
                var result = await _js.InvokeAsync<APIResponse<T>>(function, args);
                return result ?? APIResponse<T>.FailureResponse("Помилка обробки відповіді сервера.");
            }
            catch (InvalidOperationException)
            {
                // Prerender: JS ще недоступний
                return APIResponse<T>.FailureResponse("Інтерфейс ще завантажується. Спробуйте ще раз.");
            }
            catch (Exception)
            {
                return APIResponse<T>.FailureResponse("Помилка з'єднання. Спробуйте ще раз.");
            }
        }
    }
}
