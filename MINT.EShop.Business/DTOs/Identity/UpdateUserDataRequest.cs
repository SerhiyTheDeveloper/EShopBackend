namespace MINT.EShop.Business.DTOs.Identity
{
    public record UpdateUserDataRequest
    {
        /// <summary>
        /// Ім'я.
        /// </summary>
        /// <example>Валентин</example>
        public required string FirstName { get; init; }
        /// <summary>
        /// Прізвище.
        /// </summary>
        /// <example>Стрикало</example>
        public string? LastName { get; init; }
        /// <summary>
        /// Номер телефону.
        /// </summary>
        /// <example>0937026525</example>
        public required string PhoneNumber { get; init; }
    }
}
