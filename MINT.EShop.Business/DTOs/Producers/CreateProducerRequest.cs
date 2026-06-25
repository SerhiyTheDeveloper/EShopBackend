namespace MINT.EShop.Business.DTOs.Producers
{
    public record CreateProducerRequest
    {
        /// <summary>
        /// Назва виробника.
        /// </summary>
        /// <example>Apple</example>
        public required string Name { get; init; }
        /// <summary>
        /// Слаг виробника.
        /// </summary>
        /// <example>apple</example>
        public required string Slug { get; init; }

    }
}
