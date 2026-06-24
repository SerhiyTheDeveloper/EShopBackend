using System.ComponentModel.DataAnnotations;

namespace MINT.EShop.Business.DTOs.Products
{
    public record CreateProductRequest
    {
        /// <summary>
        /// Назва товару.
        /// </summary>
        /// <example>iPhone 15 Pro</example>
        public required string Name { get; init; }
        /// <summary>
        /// Опис товару.
        /// </summary>
        /// <example>Новий смартфон з покращеною камерою.</example>
        public required string Description { get; init; }
        /// <summary>
        /// Ціна товару.
        /// </summary>
        /// <example>15999</example>
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public required decimal Price { get; init; }
        /// <summary>
        /// Кількість товару.
        /// </summary>
        /// <example>5</example>
        [Range(1, int.MaxValue, ErrorMessage = "Stock must be greater than 0.")]
        public required int Stock { get; init; }
        /// <summary>
        /// Категорія товару.
        /// </summary>
        /// <example>550e8400-e29b-41d4-a716-446655440000</example>
        public required Guid CategoryId { get; init; }
        /// <summary>
        /// Виробник товару.
        /// </summary>
        /// <example>550e8400-e29b-41d4-a716-446655440001</example>
        public required Guid ProducerId { get; init; }

    }
}
