namespace MINT.EShop.Business.DTOs.Products
{
    public record GetProductsFilter
    {
        /// <summary>
        /// Мінімальна ціна товарів для фільтрації.
        /// </summary>
        public decimal? MinPrice { get; init; }

        /// <summary>
        /// Максимальна ціна товарів для фільтрації.
        /// </summary>
        public decimal? MaxPrice { get; init; }
        /// <summary>
        /// Категорія товарів для фільтрації.
        /// </summary>
        public string? Category { get; init; }
        /// <summary>
        /// Виробник товарів для фільтрації.
        /// </summary>
        public string? Producer { get; init; }
    }
}
