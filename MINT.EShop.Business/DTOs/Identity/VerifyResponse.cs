using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Business.DTOs.Identity
{
    public record VerifyResponse
    {
        /// <summary>
        /// Унікальний ідентифікатор користувача (GUID).
        /// </summary>
        public required Guid Id { get; init; }
        /// <summary>
        /// Електронна пошта користувача.
        /// </summary>
        /// <example>user@example.com</example>
        public required string Email { get; init; }
        /// <summary>
        /// Ім'я
        /// </summary>
        /// <example>Марія</example>
        public required string FirstName { get; init; }
        /// <summary>
        /// Прізвище (може бути відсутнім)
        /// </summary>
        /// <example>Кузьма</example>
        public string? LastName { get; init; }
        /// <summary>
        /// Номер телефону користувача.
        /// </summary>
        /// <example>0937026525</example>
        public required string PhoneNumber { get; init; }
        /// <summary>
        /// Токени доступу та оновлення, які користувач отримує після успішної верифікації.
        /// </summary>
        public required LoginResponse LoginResponse { get; init; }
    }
}
