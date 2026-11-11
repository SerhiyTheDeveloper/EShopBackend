using MINT.EShop.Business.DTOs.Products;
using MINT.EShop.Business.Interfaces;
using MINT.EShop.Core.Entities.Product;
using MINT.EShop.Core.Entities.UserData;
using MINT.EShop.Core.Interfaces;

namespace MINT.EShop.Business.Services
{
    public class WishListService(IUnitOfWork unitOfWork) : IWishListService
    {
        public async Task<IEnumerable<ProductResponse>> GetAllAsync(Guid clientId)
        {
            // Отримуємо всі позиції списку вподобань клієнта
            var items = await unitOfWork.WishlistItems.GetAllAsync(clientId);

            // Завантажуємо відповідні товари разом з категорією та виробником
            var products = await unitOfWork.Products.GetByIdsAsync(items.Select(i => i.ProductId));

            // Повертаємо послідовність товарів у форматі ProductResponse
            return products.Select(ToResponse);
        }

        public async Task<ProductResponse?> GetByIdAsync(Guid clientId, Guid productId)
        {
            // Перевіряємо, чи є товар у списку вподобань клієнта
            var item = await unitOfWork.WishlistItems.GetByIdAsync(clientId, productId);
            if (item == null) return null;

            // Дістаємо товар та перевіряємо його існування
            var product = await unitOfWork.Products.GetByIdAsync(productId);
            if (product == null) return null;

            // Повертаємо товар у форматі ProductResponse
            return ToResponse(product);
        }

        public async Task AddAsync(Guid clientId, Guid productId)
        {
            // Перевіряємо, чи існує товар
            var product = await unitOfWork.Products.GetByIdAsync(productId)
                ?? throw new KeyNotFoundException("Product not found.");

            // Якщо товар уже у списку вподобань, нічого не робимо (повторне додавання безпечне)
            var existing = await unitOfWork.WishlistItems.GetByIdAsync(clientId, product.Id);
            if (existing != null) return;

            // Додаємо товар до списку вподобань клієнта
            await unitOfWork.WishlistItems.AddAsync(new WishListItem
            {
                ClientId = clientId,
                ProductId = product.Id
            });

            // Завершуємо транзакцію, щоб зберегти зміни в базі даних
            await unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(Guid clientId, Guid productId)
        {
            // Якщо товару немає у списку вподобань, нічого не робимо (повторне видалення безпечне)
            var item = await unitOfWork.WishlistItems.GetByIdAsync(clientId, productId);
            if (item == null) return;

            // Видаляємо товар зі списку вподобань
            unitOfWork.WishlistItems.Delete(item);

            // Завершуємо транзакцію, щоб зберегти зміни в базі даних
            await unitOfWork.CompleteAsync();
        }

        private static ProductResponse ToResponse(Product product)
        {
            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                Category = product.Category.Name,
                Producer = product.Producer.Name,
                ImageUrl = product.ImageUrl
            };
        }
    }
}
