namespace MINT.EShop.Business.DTOs.Category
{
    public record CategoryResponse
    {
        /// <summary>
        /// Унікальний ідентифікатор категорії (GUID).
        /// </summary>
        public required Guid Id { get; init; }
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
