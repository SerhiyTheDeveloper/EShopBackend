using MINT.EShop.Business.DTOs.Products;
using MINT.EShop.Business.Interfaces;
using MINT.EShop.Core.Entities;
using MINT.EShop.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Business.Services
{
    public class ProductService(IUnitOfWork unitOfWork) : IProductService
    {
        public async Task<ProductResponse> CreateAsync(CreateProductRequest request)
        {
            // Створюємо продукт
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Stock = request.Stock
            };

            // Зберігаємо продукт у репозиторії
            await unitOfWork.Products.AddAsync(product);

            // Завершуємо транзакцію, щоб зберегти зміни в базі даних
            await unitOfWork.CompleteAsync();

            // Повертаємо відповідь у форматі ProductResponse
            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            // Отримуємо існуючий продукт за ID та перевіряємо на null
            var existingProduct = await unitOfWork.Products.GetByIdAsync(id);
            if (existingProduct == null) return false;

            // Видаляємо продукт з репозиторію та повертаємо true
            unitOfWork.Products.Delete(existingProduct);

            // Завершуємо транзакцію, щоб зберегти зміни в базі даних
            await unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<IEnumerable<ProductResponse>> GetAllAsync()
        {
            // Отримуємо список всіх продуктів з репозиторію
            var products = await unitOfWork.Products.GetAllAsync();

            // Повертаємо послідовність продуктів у форматі ProductResponse
            return products.Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock
            });
        }

        public async Task<ProductResponse?> GetByIdAsync(Guid id)
        {
            // Отримуємо продукт за ID
            var product = await unitOfWork.Products.GetByIdAsync(id);

            // Якщо продукт не знайдено, повертаємо null
            if (product == null) return null;

            // Повертаємо відповідь у форматі ProductResponse
            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock
            };
        }

        public async Task<IEnumerable<ProductResponse>> GetByIdsAsync(IEnumerable<Guid> ids)
        {
            // Отримуємо список продуктів за їх ID
            var products = await unitOfWork.Products.GetByIdsAsync(ids);

            // Повертаємо послідовність продуктів у форматі ProductResponse
            return products.Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock
            });
        }

        public async Task<ProductResponse?> UpdateAsync(Guid id, UpdateProductRequest request)
        {
            // Отримуємо існуючий продукт за ID та перевіряємо на null
            var existingProduct = await unitOfWork.Products.GetByIdAsync(id);
            if (existingProduct == null)
                return null;

            // Оновлюємо властивості продукту на основі даних з запиту
            existingProduct.Name = request.Name;
            existingProduct.Description = request.Description;
            existingProduct.Price = request.Price;

            // Завершуємо транзакцію, щоб зберегти зміни в базі даних
            await unitOfWork.CompleteAsync();

            // Повертаємо оновлений продукт у форматі ProductResponse
            return new ProductResponse()
            {
                Id = existingProduct.Id,
                Name = existingProduct.Name,
                Description = existingProduct.Description,
                Price = existingProduct.Price,
                Stock = existingProduct.Stock
            };
        }
    }
}