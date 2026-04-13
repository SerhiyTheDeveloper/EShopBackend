using System.ComponentModel.DataAnnotations;

namespace MINT.EShop.Business.DTOs.Identity
{
    /// <summary>
    /// Дані для входу в систему
    /// </summary>
    public record LoginRequest
    {
        /// <summary>
        /// Електронна пошта.
        /// </summary>
        /// <example>user@example.com</example>
        public required string Email { get; init; }
        /// <summary>
        /// Пароль.
        /// </summary>
        /// <example>StrPassword123</example>
        [MinLength(8)]
        public required string Password { get; init; }
    }
}
