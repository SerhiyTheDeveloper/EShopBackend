using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using MINT.EShop.API.Wrappers;
using MINT.EShop.Business.DTOs.Products;

namespace Web.Services
{
    /// <summary>
    /// Вподобання поточного користувача в межах circuit. Один екземпляр спільний для сердечок на картках,
    /// сторінки товару, сторінки «Вподобання» та значка в шапці. Запити йдуть через fetch до Web
    /// (/api/wishlist), де токен оновлюється непомітно: circuit не може записати cookie.
    /// </summary>
    public class WishListStateService(IJSRuntime js, AuthenticationStateProvider authProvider, UiStateService uiState)
    {
        private readonly IJSRuntime _js = js;
        private readonly AuthenticationStateProvider _authProvider = authProvider;
        private readonly UiStateService _uiState = uiState;

        private readonly List<ProductResponse> _products = [];
        private readonly HashSet<Guid> _ids = [];
        private readonly HashSet<Guid> _pending = [];
        private Task? _loadTask;

        public IReadOnlyList<ProductResponse> Products => _products;
        public int Count => _products.Count;
        public bool IsLoaded { get; private set; }
        public event Action? OnChange;

        public bool Contains(Guid productId) => _ids.Contains(productId);

        /// <summary>
        /// Завантажує список один раз. Гість нічого не завантажує; у prerender JS недоступний,
        /// тоді завантаження повториться вже в circuit.
        /// </summary>
        public Task EnsureLoadedAsync()
        {
            if (IsLoaded) return Task.CompletedTask;
            if (_loadTask is { IsCompleted: false }) return _loadTask;

            return _loadTask = LoadAsync();
        }

        /// <summary>
        /// Додає товар до вподобань або прибирає його. Гостя просить увійти. Повертає текст помилки або null.
        /// Зміна показується одразу, а при помилці API відкочується.
        /// </summary>
        public async Task<string?> ToggleAsync(ProductResponse product)
        {
            if (!await IsAuthenticatedAsync())
            {
                _uiState.Show(Modal.Login);
                return null;
            }

            // Повторний клік, поки попередній запит ще виконується, ігноруємо
            if (!_pending.Add(product.Id)) return null;

            try
            {
                await EnsureLoadedAsync();

                var adding = !Contains(product.Id);
                ApplyLocal(product, adding);

                var response = await CallAsync<object>(adding ? "POST" : "DELETE", product.Id.ToString());
                if (response.Success) return null;

                ApplyLocal(product, !adding);

                if (response.Message == ApiClientBase.SessionExpiredMessage)
                {
                    _uiState.Show(Modal.Login);
                    return null;
                }

                return string.IsNullOrWhiteSpace(response.Message)
                    ? "Не вдалося оновити вподобання. Спробуйте ще раз."
                    : response.Message;
            }
            finally
            {
                _pending.Remove(product.Id);
            }
        }

        private async Task LoadAsync()
        {
            if (!await IsAuthenticatedAsync()) return;

            var response = await CallAsync<List<ProductResponse>>("GET", null);
            if (!response.Success || response.Data is null) return;

            _products.Clear();
            _products.AddRange(response.Data);
            _ids.Clear();
            foreach (var product in response.Data) _ids.Add(product.Id);

            IsLoaded = true;
            OnChange?.Invoke();
        }

        private void ApplyLocal(ProductResponse product, bool add)
        {
            if (add)
            {
                if (_ids.Add(product.Id)) _products.Add(product);
            }
            else
            {
                _ids.Remove(product.Id);
                _products.RemoveAll(p => p.Id == product.Id);
            }

            OnChange?.Invoke();
        }

        private async Task<bool> IsAuthenticatedAsync()
        {
            var state = await _authProvider.GetAuthenticationStateAsync();
            return state.User.Identity?.IsAuthenticated == true;
        }

        private async Task<APIResponse<T>> CallAsync<T>(string method, string? path)
        {
            try
            {
                var result = await _js.InvokeAsync<APIResponse<T>>("callWishListApi", method, path);
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
