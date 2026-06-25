namespace MINT.EShop.Business.DTOs.Producers
{
    public record UpdateProducerRequest
    {
        /// <summary>
        /// Нова назва виробника.
        /// </summary>
        /// <example>Apple</example>
        public required string Name { get; init; }
        /// <summary>
        /// Новий слаг виробника.
        /// </summary>
        /// <example>apple</example>
        public required string Slug { get; init; }
    }
}
