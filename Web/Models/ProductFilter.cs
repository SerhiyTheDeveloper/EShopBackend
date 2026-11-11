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

        public bool HasPrice => MinPrice.HasValue || MaxPrice.HasValue;

        public bool IsActive => HasPrice || Category is not null || Brands.Count > 0;

        /// <summary>Кількість активних умов для значка на кнопці (ціна рахується як одна).</summary>
        public int ActiveCount => (HasPrice ? 1 : 0) + (Category is not null ? 1 : 0) + Brands.Count;

        public bool Matches(ProductResponse product)
            => (!MinPrice.HasValue || product.Price >= MinPrice.Value)
               && (!MaxPrice.HasValue || product.Price <= MaxPrice.Value)
               && (Category is null || product.Category.Equals(Category, StringComparison.OrdinalIgnoreCase))
               && (Brands.Count == 0 || Brands.Contains(product.Producer));

        public ProductFilter Clone() => new()
        {
            MinPrice = MinPrice,
            MaxPrice = MaxPrice,
            Category = Category,
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
