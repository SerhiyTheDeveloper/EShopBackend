using System.ComponentModel.DataAnnotations;

namespace MINT.EShop.Business.DTOs.Identity
{
    public record RegisterRequest
    {
        /// <summary>
        /// Електронна пошта користувача.
        /// </summary>
        /// <example>user@example.com</example>
        public required string Email { get; init; }
        /// <summary>
        /// Пароль користувача(мінімум 8 символів).
        /// </summary>
        /// <example>StrPassword123</example>
        [MinLength(8)]
        public required string Password { get; init; }
        /// <summary>
        /// Ім'я.
        /// </summary>
        /// <example>Іван</example>
        public required string FirstName { get; init; }
        /// <summary>
        /// Прізвище.
        /// </summary>
        /// <example>Бондар</example>
        public string? LastName { get; init; }
    }
}
