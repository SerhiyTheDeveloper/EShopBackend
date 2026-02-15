using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Business.DTOs.Products
{
    public record ProductResponse
    {
        /// <summary>
        /// Унікальний ідентифікатор товару (GUID).
        /// </summary>
        public required Guid Id { get; init; }
        /// <summary>
        /// Назва товару.
        /// </summary>
        /// <example>Смартфон Samsung Galaxy S23</example>
        public required string Name { get; init; }
        /// <summary>
        /// Опис товару.
        /// </summary>
        /// <example>Новий смартфон з потужним процесором.</example>
        public string? Description { get; init; }
        /// <summary>
        /// Ціна товару.
        /// </summary>
        /// <example>26999</example>
        public required decimal Price { get; init; }
    }
}
