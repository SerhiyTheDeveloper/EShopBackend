using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Business.DTOs.Identity
{
    public record RegisterRequest
    {
        /// <summary>
        /// Електронна пошта користувача.
        /// </summary>
        /// <example>user@example.com</example>
        [Required]
        public required string Email { get; init; }
        /// <summary>Пароль користувача(мінімум 6 символів).</summary>
        [Required]
        [MinLength(6)]
        public required string Password { get; init; }
        /// <summary>Підтвердження пароля користувача.</summary>
        [Required]
        [MinLength(6)]
        [Compare("Password", ErrorMessage = "Паролі не збігаються")]
        public required string ConfirmPassword { get; init; }
    }
}
