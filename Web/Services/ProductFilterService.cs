using MINT.EShop.Business.DTOs.Category;
using MINT.EShop.Business.DTOs.Products;
using Web.Models;

namespace Web.Services
{
    /// <summary>
    /// Каталог для головної та фільтр товарів. Один екземпляр на circuit: головна показує
    /// відфільтрований список, FilterDrawer змінює критерії, Header показує значок активного фільтра.
    /// Товарів небагато і вони вже завантажуються повністю, тому фільтрація робиться тут, а не на API
    /// (API вміє лише одну категорію та одного виробника, а тут можна обрати кілька брендів).
    /// </summary>
    public class ProductFilterService(CatalogApiClient catalog)
    {
        private readonly CatalogApiClient _catalog = catalog;
        private Task? _loadTask;

        public IReadOnlyList<ProductResponse> Products { get; private set; } = [];
        public IReadOnlyList<CategoryResponse> Categories { get; private set; } = [];
        public bool IsLoaded { get; private set; }
        public string? LoadError { get; private set; }

        /// <summary>Застосований фільтр (те, що бачить головна).</summary>
        public ProductFilter Applied { get; private set; } = new();

        public event Action? OnChange;

        public IEnumerable<ProductResponse> Filtered => Products.Where(Applied.Matches);

        /// <summary>Завантажує каталог один раз. Паралельні виклики ділять один запит.</summary>
        public Task EnsureLoadedAsync()
        {
            if (IsLoaded) return Task.CompletedTask;
            if (_loadTask is { IsCompleted: false }) return _loadTask;

            return _loadTask = LoadAsync();
        }

        public Task ReloadAsync()
        {
            IsLoaded = false;
            return EnsureLoadedAsync();
        }

        public void Apply(ProductFilter filter)
        {
            Applied = filter.Clone();
            OnChange?.Invoke();
        }

        public void Reset() => Apply(new ProductFilter());

        public void RemovePrice()
        {
            var filter = Applied.Clone();
            filter.MinPrice = null;
            filter.MaxPrice = null;
            Apply(filter);
        }

        public void RemoveCategory()
        {
            var filter = Applied.Clone();
            filter.Category = null;
            Apply(filter);
        }

        public void RemoveBrand(string brand)
        {
            var filter = Applied.Clone();
            filter.Brands.Remove(brand);
            Apply(filter);
        }

        public int Count(ProductFilter filter) => Products.Count(filter.Matches);

        /// <summary>Усі бренди (виробники), що мають товари в каталозі.</summary>
        public IReadOnlyList<string> AllBrands()
            => Products
                .Select(p => p.Producer)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(b => b)
                .ToList();

        /// <summary>Найменша та найбільша ціна в каталозі (для підказок у полях ціни).</summary>
        public (decimal Min, decimal Max)? PriceBounds()
            => Products.Count == 0 ? null : (Products.Min(p => p.Price), Products.Max(p => p.Price));

        private async Task LoadAsync()
        {
            var products = await _catalog.GetProductsAsync();
            var categories = await _catalog.GetCategoriesAsync();

            if (products.Success && products.Data is not null && categories.Success && categories.Data is not null)
            {
                Products = products.Data;
                Categories = categories.Data;
                IsLoaded = true;
                LoadError = null;
            }
            else
            {
                var message = !string.IsNullOrWhiteSpace(products.Message) ? products.Message : categories.Message;
                LoadError = string.IsNullOrWhiteSpace(message) ? "Не вдалося завантажити товари." : message;
            }

            OnChange?.Invoke();
        }
    }
}
