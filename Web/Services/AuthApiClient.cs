using MINT.EShop.API.Wrappers;
using MINT.EShop.Business.DTOs.Identity;
using System.Text.Json;

namespace Web.Services
{
    public class AuthApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public AuthApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<APIResponse<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancToken = default)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/v1/auth/login", request, _jsonOptions, cancToken);
                if (response.IsSuccessStatusCode)
                {
                    var successResult = await response.Content.ReadFromJsonAsync<APIResponse<LoginResponse>>(_jsonOptions, cancToken);
                    return successResult ?? APIResponse<LoginResponse>.FailureResponse("Помилка обробки відповіді сервера.");
                }

                try
                {
                    var errorResult = await response.Content.ReadFromJsonAsync<APIResponse<LoginResponse>>(_jsonOptions, cancToken);
                    if (errorResult != null)
                    {
                        return errorResult;
                    }
                }
                catch (JsonException)
                {
                    // Помилка десеріалізації JSON при авторизації
                }

                // Запасний варіант, якщо сервер повернув помилку без ApiResponse
                return response.StatusCode switch
                {
                    System.Net.HttpStatusCode.BadRequest => APIResponse<LoginResponse>.FailureResponse("Некоректний запит. Перевірте введені дані."),
                    System.Net.HttpStatusCode.Unauthorized => APIResponse<LoginResponse>.FailureResponse("Неправильний email або пароль."),
                    System.Net.HttpStatusCode.Forbidden => APIResponse<LoginResponse>.FailureResponse("Доступ заборонено."),
                    System.Net.HttpStatusCode.InternalServerError => APIResponse<LoginResponse>.FailureResponse("Сервер тимчасово недоступний. Спробуйте пізніше."),
                    _ => APIResponse<LoginResponse>.FailureResponse($"Помилка сервера. Статус-код: {response.StatusCode}")
                };
            }
            catch (OperationCanceledException)
            {
                return APIResponse<LoginResponse>.FailureResponse("Перевищено час очікування відповіді. Спробуйте знову.");
            }
            catch (HttpRequestException)
            {
                return APIResponse<LoginResponse>.FailureResponse("Не вдалося зв'язатися з сервером. Перевірте підключення.");
            }
            catch (Exception ex)
            {
                return APIResponse<LoginResponse>.FailureResponse($"Сталася непередбачувана помилка: {ex.Message}");
            }
        }

        public async Task<APIResponse<RegisterResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancToken = default)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/v1/Users/register", request, _jsonOptions, cancToken);

                if (response.IsSuccessStatusCode)
                {
                    var successResult = await response.Content.ReadFromJsonAsync<APIResponse<RegisterResponse>>(_jsonOptions, cancToken);
                    return successResult ?? APIResponse<RegisterResponse>.FailureResponse("Помилка обробки відповіді сервера.");
                }

                try
                {
                    var errorResult = await response.Content.ReadFromJsonAsync<APIResponse<RegisterResponse>>(_jsonOptions, cancToken);
                    if (errorResult != null)
                    {
                        return errorResult;
                    }
                }
                catch (JsonException)
                {
                    // Помилка десеріалізації JSON при реєстрації
                }

                // Запасний варіант, якщо сервер повернув помилку без ApiResponse
                return response.StatusCode switch
                {
                    System.Net.HttpStatusCode.BadRequest => APIResponse<RegisterResponse>.FailureResponse("Некоректний запит. Перевірте введені дані."),
                    System.Net.HttpStatusCode.InternalServerError => APIResponse<RegisterResponse>.FailureResponse("Сервер тимчасово недоступний. Спробуйте пізніше."),
                    _ => APIResponse<RegisterResponse>.FailureResponse($"Помилка сервера. Статус-код: {response.StatusCode}")
                };
            }
            catch (OperationCanceledException)
            {
                return APIResponse<RegisterResponse>.FailureResponse("Перевищено час очікування відповіді. Спробуйте знову.");
            }
            catch (HttpRequestException)
            {
                return APIResponse<RegisterResponse>.FailureResponse("Не вдалося зв'язатися з сервером. Перевірте підключення.");
            }
            catch (Exception ex)
            {
                return APIResponse<RegisterResponse>.FailureResponse($"Сталася непередбачувана помилка: {ex.Message}");
            }
        }

        public async Task<APIResponse<RegisterResponse>> ResentAsync(ResentRequest request, CancellationToken cancToken = default)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/v1/Users/resent", request, _jsonOptions, cancToken);

                if (response.IsSuccessStatusCode)
                {
                    var successResult = await response.Content.ReadFromJsonAsync<APIResponse<RegisterResponse>>(_jsonOptions, cancToken);
                    return successResult ?? APIResponse<RegisterResponse>.FailureResponse("Помилка обробки відповіді сервера.");
                }

                try
                {
                    var errorResult = await response.Content.ReadFromJsonAsync<APIResponse<RegisterResponse>>(_jsonOptions, cancToken);
                    if (errorResult != null)
                    {
                        return errorResult;
                    }
                }
                catch (JsonException)
                {
                    // Помилка десеріалізації JSON при реєстрації
                }

                // Запасний варіант, якщо сервер повернув помилку без ApiResponse
                return response.StatusCode switch
                {
                    System.Net.HttpStatusCode.NotFound => APIResponse<RegisterResponse>.FailureResponse("Час коду вичерпано. Зареєструйтесь заново."),
                    System.Net.HttpStatusCode.BadRequest => APIResponse<RegisterResponse>.FailureResponse("Даний користувач вже існує. Увійдть."),
                    System.Net.HttpStatusCode.InternalServerError => APIResponse<RegisterResponse>.FailureResponse("Сервер тимчасово недоступний. Спробуйте пізніше."),
                    _ => APIResponse<RegisterResponse>.FailureResponse($"Помилка сервера. Статус-код: {response.StatusCode}")
                };
            }
            catch (OperationCanceledException)
            {
                return APIResponse<RegisterResponse>.FailureResponse("Перевищено час очікування відповіді. Спробуйте знову.");
            }
            catch (HttpRequestException)
            {
                return APIResponse<RegisterResponse>.FailureResponse("Не вдалося зв'язатися з сервером. Перевірте підключення.");
            }
            catch (Exception ex)
            {
                return APIResponse<RegisterResponse>.FailureResponse($"Сталася непередбачувана помилка: {ex.Message}");
            }
        }
        public async Task<APIResponse<VerifyResponse>> VerifyAsync(VerifyRequest request, CancellationToken cancToken = default)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/v1/Users/verify", request, _jsonOptions, cancToken);

                if (response.IsSuccessStatusCode)
                {
                    var successResult = await response.Content.ReadFromJsonAsync<APIResponse<VerifyResponse>>(_jsonOptions, cancToken);
                    return successResult ?? APIResponse<VerifyResponse>.FailureResponse("Помилка обробки відповіді сервера.");
                }

                try
                {
                    var errorResult = await response.Content.ReadFromJsonAsync<APIResponse<VerifyResponse>>(_jsonOptions, cancToken);
                    if (errorResult != null)
                    {
                        return errorResult;
                    }
                }
                catch (JsonException)
                {
                    // Помилка десеріалізації JSON при реєстрації
                }

                // Запасний варіант, якщо сервер повернув помилку без ApiResponse
                return response.StatusCode switch
                {
                    System.Net.HttpStatusCode.BadRequest => APIResponse<VerifyResponse>.FailureResponse("Некоректний запит. Перевірте введені дані."),
                    System.Net.HttpStatusCode.InternalServerError => APIResponse<VerifyResponse>.FailureResponse("Сервер тимчасово недоступний. Спробуйте пізніше."),
                    _ => APIResponse<VerifyResponse>.FailureResponse($"Помилка сервера. Статус-код: {response.StatusCode}")
                };
            }
            catch (OperationCanceledException)
            {
                return APIResponse<VerifyResponse>.FailureResponse("Перевищено час очікування відповіді. Спробуйте знову.");
            }
            catch (HttpRequestException)
            {
                return APIResponse<VerifyResponse>.FailureResponse("Не вдалося зв'язатися з сервером. Перевірте підключення.");
            }
            catch (Exception ex)
            {
                return APIResponse<VerifyResponse>.FailureResponse($"Сталася непередбачувана помилка: {ex.Message}");
            }
        }
    }
}
