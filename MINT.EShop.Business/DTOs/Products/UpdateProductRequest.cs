using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Business.DTOs.Products
{
    public record UpdateProductRequest
    {
        /// <summary>
        /// Назва товару.
        /// </summary>
        /// <example>iPhone 15 Pro</example>
        public required string Name { get; init; }
        /// <summary>
        /// Опис товару.
        /// </summary>
        /// <example>Новий смартфон з покращеною камерою.</example>
        public required string Description { get; init; }
        /// <summary>
        /// Ціна товару.
        /// </summary>
        /// <example>15999</example>
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public required decimal Price { get; init; }
        /// <summary>
        /// Кількість товару.
        /// </summary>
        /// <example>5</example>
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be at least 0.")]
        public required int Stock { get; init; }
    }
}
