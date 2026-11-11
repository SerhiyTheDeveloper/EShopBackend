using MINT.EShop.API.Wrappers;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Web.Services
{
    /// <summary>
    /// Спільна частина клієнтів до API, що вимагають авторизації. Access token передається явно:
    /// його надають або fetch-endpoints Web (завжди свіжий), або сервіси з cookie поточного circuit.
    /// </summary>
    public abstract class ApiClientBase(HttpClient httpClient)
    {
        /// <summary>
        /// Повідомлення, коли токен прострочений або API відповів 401.
        /// </summary>
        public const string SessionExpiredMessage = "Сесія завершилась. Увійдіть знову.";

        private readonly HttpClient _httpClient = httpClient;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        protected async Task<APIResponse<T>> SendAsync<T>(HttpMethod method, string url, string accessToken, object? body, CancellationToken cancToken)
        {
            try
            {
                using var request = new HttpRequestMessage(method, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                if (body is not null)
                {
                    request.Content = JsonContent.Create(body, body.GetType(), options: _jsonOptions);
                }

                using var response = await _httpClient.SendAsync(request, cancToken);
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return APIResponse<T>.FailureResponse(SessionExpiredMessage);
                }

                if (response.IsSuccessStatusCode)
                {
                    var successResult = await response.Content.ReadFromJsonAsync<APIResponse<T>>(_jsonOptions, cancToken);
                    return successResult ?? APIResponse<T>.FailureResponse("Помилка обробки відповіді сервера.");
                }

                try
                {
                    var errorResult = await response.Content.ReadFromJsonAsync<APIResponse<T>>(_jsonOptions, cancToken);
                    if (errorResult != null)
                    {
                        return errorResult;
                    }
                }
                catch (JsonException)
                {
                    // Помилка десеріалізації JSON у відповіді з помилкою
                }

                // Запасний варіант, якщо сервер повернув помилку без ApiResponse
                return response.StatusCode switch
                {
                    HttpStatusCode.BadRequest => APIResponse<T>.FailureResponse("Некоректний запит. Перевірте введені дані."),
                    HttpStatusCode.Forbidden => APIResponse<T>.FailureResponse("Недостатньо прав для цієї дії."),
                    HttpStatusCode.NotFound => APIResponse<T>.FailureResponse("Не знайдено."),
                    HttpStatusCode.InternalServerError => APIResponse<T>.FailureResponse("Сервер тимчасово недоступний. Спробуйте пізніше."),
                    _ => APIResponse<T>.FailureResponse($"Помилка сервера. Статус-код: {response.StatusCode}")
                };
            }
            catch (OperationCanceledException)
            {
                return APIResponse<T>.FailureResponse("Перевищено час очікування відповіді. Спробуйте знову.");
            }
            catch (HttpRequestException)
            {
                return APIResponse<T>.FailureResponse("Не вдалося зв'язатися з сервером. Перевірте підключення.");
            }
            catch (Exception ex)
            {
                return APIResponse<T>.FailureResponse($"Сталася непередбачувана помилка: {ex.Message}");
            }
        }
    }
}
