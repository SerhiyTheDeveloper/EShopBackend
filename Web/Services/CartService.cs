using Microsoft.JSInterop;
using MINT.EShop.Business.DTOs.Products;
using Web.Models;

namespace Web.Services
{
    public enum CartAddResult
    {
        Added,
        LimitReached,
        OutOfStock
    }

    /// <summary>
    /// Кошик у браузері (localStorage): працює і для гостей, і без API кошика. Зберігаються лише
    /// ідентифікатори та кількості. Один екземпляр спільний для карток, сторінки товару, сторінки кошика та шапки.
    /// </summary>
    public class CartService(IJSRuntime js)
    {
        private readonly IJSRuntime _js = js;
        private readonly List<CartItem> _items = [];
        private Task? _loadTask;

        public IReadOnlyList<CartItem> Items => _items;

        /// <summary>Загальна кількість одиниць товару в кошику (для значка).</summary>
        public int Count => _items.Sum(i => i.Quantity);

        public bool IsLoaded { get; private set; }
        public event Action? OnChange;

        public bool Contains(Guid productId) => _items.Any(i => i.ProductId == productId);

        public int QuantityOf(Guid productId) => _items.FirstOrDefault(i => i.ProductId == productId)?.Quantity ?? 0;

        /// <summary>
        /// Читає кошик із localStorage один раз. У prerender JS недоступний, тоді читання повториться в circuit.
        /// </summary>
        public Task EnsureLoadedAsync()
        {
            if (IsLoaded) return Task.CompletedTask;
            if (_loadTask is { IsCompleted: false }) return _loadTask;

            return _loadTask = LoadAsync();
        }

        /// <summary>Додає товар. Кількість обмежена залишком на складі.</summary>
        public async Task<CartAddResult> AddAsync(ProductResponse product, int quantity = 1)
        {
            await EnsureLoadedAsync();

            if (product.Stock <= 0) return CartAddResult.OutOfStock;

            var current = QuantityOf(product.Id);
            var wanted = current + Math.Max(quantity, 1);
            var target = Math.Min(wanted, product.Stock);

            if (target == current) return CartAddResult.LimitReached;

            Upsert(product.Id, target);
            await SaveAsync();
            OnChange?.Invoke();

            return target < wanted ? CartAddResult.LimitReached : CartAddResult.Added;
        }

        public async Task SetQuantityAsync(Guid productId, int quantity)
        {
            if (quantity <= 0)
            {
                await RemoveAsync(productId);
                return;
            }

            Upsert(productId, quantity);
            await SaveAsync();
            OnChange?.Invoke();
        }

        public async Task RemoveAsync(Guid productId)
        {
            if (_items.RemoveAll(i => i.ProductId == productId) == 0) return;

            await SaveAsync();
            OnChange?.Invoke();
        }

        public async Task ClearAsync()
        {
            if (_items.Count == 0) return;

            _items.Clear();
            await SaveAsync();
            OnChange?.Invoke();
        }

        /// <summary>
        /// Звіряє кошик із актуальним каталогом: прибирає товари, яких уже немає,
        /// і зменшує кількість до залишку, якщо він став меншим.
        /// </summary>
        public async Task ReconcileAsync(IReadOnlyCollection<ProductResponse> products)
        {
            var byId = products.ToDictionary(p => p.Id);
            var changed = false;

            foreach (var item in _items.ToList())
            {
                if (!byId.TryGetValue(item.ProductId, out var product))
                {
                    _items.Remove(item);
                    changed = true;
                }
                else if (product.Stock > 0 && item.Quantity > product.Stock)
                {
                    Upsert(item.ProductId, product.Stock);
                    changed = true;
                }
            }

            if (!changed) return;

            await SaveAsync();
            OnChange?.Invoke();
        }

        private void Upsert(Guid productId, int quantity)
        {
            var index = _items.FindIndex(i => i.ProductId == productId);
            var item = new CartItem(productId, quantity);

            if (index >= 0) _items[index] = item;
            else _items.Add(item);
        }

        private async Task LoadAsync()
        {
            try
            {
                var stored = await _js.InvokeAsync<List<CartItem>>("cartLoad");

                _items.Clear();
                foreach (var item in stored ?? [])
                {
                    // Пошкоджені записи пропускаємо
                    if (item.ProductId != Guid.Empty && item.Quantity > 0) Upsert(item.ProductId, item.Quantity);
                }

                IsLoaded = true;
                OnChange?.Invoke();
            }
            catch (InvalidOperationException)
            {
                // Prerender: JS ще недоступний, повторимо в circuit
            }
            catch (Exception)
            {
                IsLoaded = true;
            }
        }

        private async Task SaveAsync()
        {
            try
            {
                await _js.InvokeVoidAsync("cartSave", _items);
            }
            catch (Exception)
            {
                // localStorage недоступний або заповнений: кошик лишається в пам'яті до перезавантаження
            }
        }
    }
}
