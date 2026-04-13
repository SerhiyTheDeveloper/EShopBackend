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
    }
}
