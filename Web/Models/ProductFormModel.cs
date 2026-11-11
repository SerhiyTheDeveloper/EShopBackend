using MINT.EShop.Business.DTOs.Products;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class ProductFormModel
    {
        [Required(ErrorMessage = "Введіть назву")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть опис")]
        public string Description { get; set; } = string.Empty;

        [Range(0.01, 1_000_000_000, ErrorMessage = "Ціна має бути більшою за 0")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int Stock { get; set; }

        // Лише при створенні (API не дозволяє змінити категорію й виробника існуючого товару)
        public Guid CategoryId { get; set; }
        public Guid ProducerId { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public void Reset()
        {
            Name = string.Empty;
            Description = string.Empty;
            Price = 0;
            Stock = 0;
            CategoryId = Guid.Empty;
            ProducerId = Guid.Empty;
            ImageUrl = string.Empty;
        }

        public void LoadFrom(ProductResponse product)
        {
            Name = product.Name;
            Description = product.Description ?? string.Empty;
            Price = product.Price;
            Stock = product.Stock;
            ImageUrl = product.ImageUrl;
        }
    }
}
