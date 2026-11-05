using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Business.DTOs.Identity
{
    public record ResentRequest
    {
        /// <summary>
        /// Електронна пошта
        /// </summary>
        /// <example>user@example.com</example>
        public required string Email { get; init; }
    }
}
