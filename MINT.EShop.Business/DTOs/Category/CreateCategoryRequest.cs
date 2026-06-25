namespace MINT.EShop.Business.DTOs.Category
{
    public record CreateCategoryRequest
    {
        /// <summary>
        /// Назва категорії.
        /// </summary>
        /// <example>Смартфони</example>
        public required string Name { get; init; }
        /// <summary>
        /// Слаг категорії.
        /// </summary>
        /// <example>smartphones</example>
        public required string Slug { get; init; }
    }
}