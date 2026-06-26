namespace MINT.EShop.Business.DTOs.WishList
{
    public record WishlistItemResponse
    {
        /// <summary>
        /// Унікальний ідентифікатор клієнта (GUID).
        /// </summary>
        /// <example>550e8400-e29b-41d4-a716-446655440000</example>
        public Guid ClientId { get; init; }

        /// <summary>
        /// Унікальний ідентифікатор продукту (GUID).
        /// </summary>
        /// <example>550e8400-e29b-41d4-a716-446655440000</example>
        public Guid ProductId { get; init; }
    }
}
