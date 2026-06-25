using MINT.EShop.Business.DTOs.Products;
using MINT.EShop.Business.Interfaces;
using MINT.EShop.Core.Entities.Product;
using MINT.EShop.Core.Interfaces;

namespace MINT.EShop.Business.Services
{
    public class ProductService(IUnitOfWork unitOfWork) : IProductService
    {
        public async Task<ProductResponse> CreateAsync(Guid managerId, CreateProductRequest request)
        {
            // Створюємо продукт
            var product = new Product
            {
                Id = Guid.NewGuid(),
                ManagerId = managerId,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Stock = request.Stock,
                CategoryId = request.CategoryId,
                ProducerId = request.ProducerId,
                ImageUrl = request.ImageUrl
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
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                ProducerId = product.ProducerId,
                ImageUrl = product.ImageUrl
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            // Отримуємо існуючий продукт за ID та перевіряємо на null
            var existingProduct = await unitOfWork.Products.GetByIdAsync(id);
            if (existingProduct == null) return false;

            // Видаляємо продукт з репозиторію
            unitOfWork.Products.Delete(existingProduct);

            // Завершуємо транзакцію, щоб зберегти зміни в базі даних
            await unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<IEnumerable<ProductResponse>> GetAllAsync(GetProductsFilter filter)
        {
            // Отримуємо список всіх продуктів з репозиторію
            var products = await unitOfWork.Products.GetAllAsync(filter.MaxPrice, filter.MinPrice, filter.Category, filter.Producer);

            // Повертаємо послідовність продуктів у форматі ProductResponse
            return products.Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                CategoryId = p.CategoryId,
                ProducerId = p.ProducerId,
                ImageUrl = p.ImageUrl
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
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                ProducerId = product.ProducerId,
                ImageUrl = product.ImageUrl
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
                Stock = p.Stock,
                CategoryId = p.CategoryId,
                ProducerId = p.ProducerId,
                ImageUrl = p.ImageUrl
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
                Stock = existingProduct.Stock,
                CategoryId = existingProduct.CategoryId,
                ProducerId = existingProduct.ProducerId,
                ImageUrl = existingProduct.ImageUrl
            };
        }
    }
}