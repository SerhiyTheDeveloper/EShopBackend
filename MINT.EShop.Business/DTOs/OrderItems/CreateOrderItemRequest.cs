using System.ComponentModel.DataAnnotations;

namespace MINT.EShop.Business.DTOs.OrderItems
{
    public record CreateOrderItemRequest
    {
        /// <summary>
        /// Унікальний ідентифікатор (GUID).
        /// </summary>
        public required Guid ProductId { get; init; }
        /// <summary>
        /// Кількість одиниць товару.
        /// </summary>
        /// <example>2</example>
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public required int Quantity { get; init; }
    }
}
