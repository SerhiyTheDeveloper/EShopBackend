using MINT.EShop.Business.DTOs.Products;

namespace Web.Models
{
    /// <summary>
    /// Критерії фільтра товарів на головній. Бренд це виробник товару.
    /// </summary>
    public sealed class ProductFilter
    {
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        /// <summary>Назва категорії (у відповіді товару є лише назва, а не Id).</summary>
        public string? Category { get; set; }

        public HashSet<string> Brands { get; private set; } = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>Текст із пошукового рядка шапки: слова шукаються в назві, бренді та категорії.</summary>
        public string? Search { get; set; }

        public bool HasSearch => !string.IsNullOrWhiteSpace(Search);

        public bool HasPrice => MinPrice.HasValue || MaxPrice.HasValue;

        public bool IsActive => HasPrice || HasSearch || Category is not null || Brands.Count > 0;

        /// <summary>Кількість умов фільтра для значка на кнопці (ціна як одна; пошук має власний рядок і не рахується).</summary>
        public int ActiveCount => (HasPrice ? 1 : 0) + (Category is not null ? 1 : 0) + Brands.Count;

        public bool Matches(ProductResponse product)
            => (!MinPrice.HasValue || product.Price >= MinPrice.Value)
               && (!MaxPrice.HasValue || product.Price <= MaxPrice.Value)
               && (Category is null || product.Category.Equals(Category, StringComparison.OrdinalIgnoreCase))
               && (Brands.Count == 0 || Brands.Contains(product.Producer))
               && MatchesSearch(product);

        // Кожне слово запиту має зустрітися в назві, бренді або категорії (регістр не важливий)
        private bool MatchesSearch(ProductResponse product)
        {
            if (!HasSearch) return true;

            var text = $"{product.Name} {product.Producer} {product.Category}";
            return Search!
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .All(word => text.Contains(word, StringComparison.OrdinalIgnoreCase));
        }

        public ProductFilter Clone() => new()
        {
            MinPrice = MinPrice,
            MaxPrice = MaxPrice,
            Category = Category,
            Search = Search,
            Brands = new HashSet<string>(Brands, StringComparer.OrdinalIgnoreCase)
        };

        /// <summary>Копія без умови за категорією (для лічильників категорій).</summary>
        public ProductFilter WithoutCategory()
        {
            var copy = Clone();
            copy.Category = null;
            return copy;
        }

        /// <summary>Копія без умови за брендами (для лічильників брендів).</summary>
        public ProductFilter WithoutBrands()
        {
            var copy = Clone();
            copy.Brands.Clear();
            return copy;
        }
    }
}
