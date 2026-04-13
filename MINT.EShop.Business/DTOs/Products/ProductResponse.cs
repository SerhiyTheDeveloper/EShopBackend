namespace MINT.EShop.Business.DTOs.Products
{
    public record ProductResponse
    {
        /// <summary>
        /// Унікальний ідентифікатор товару (GUID).
        /// </summary>
        public required Guid Id { get; init; }
        /// <summary>
        /// Назва товару.
        /// </summary>
        /// <example>Смартфон Samsung Galaxy S23</example>
        public required string Name { get; init; }
        /// <summary>
        /// Опис товару.
        /// </summary>
        /// <example>Новий смартфон з потужним процесором.</example>
        public string? Description { get; init; }
        /// <summary>
        /// Ціна товару.
        /// </summary>
        /// <example>26999.99</example>
        public required decimal Price { get; init; }
        /// <summary>
        /// Кількість товару на складі.
        /// </summary>
        /// <example>10</example>
        public required int Stock { get; init; }
    }
}
