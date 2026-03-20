using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Business.DTOs.Identity
{
    /// <summary>
    /// Відповідь після успішної автентифікації користувача
    /// </summary>
    public record LoginResponse
    {
        /// <summary>
        /// Токен для автентифікації запитів
        /// </summary>
        public required string AccessToken { get; init; }
        /// <summary>
        /// Токен для оновлення сесії після закінчення дії AccessToken
        /// </summary>
        public required string RefreshToken { get; init; }
        /// <summary>
        /// Дата та час закінчення дії RefreshToken (UTC)
        /// </summary>
        public required DateTime ExpiresDate { get; init; }
    }
}
