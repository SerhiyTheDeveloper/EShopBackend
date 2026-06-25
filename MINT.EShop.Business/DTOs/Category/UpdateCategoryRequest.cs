namespace MINT.EShop.Business.DTOs.Category
{
    public record UpdateCategoryRequest
    {
        /// <summary>
        /// Нова назва категорії.
        /// </summary>
        /// <example>Смартфони</example>
        public required string Name { get; init; }
        /// <summary>
        /// Новий слаг категорії.
        /// </summary>
        /// <example>smartphones</example>
        public required string Slug { get; init; }
    }
}
