using MINT.EShop.Business.DTOs.Category;
using MINT.EShop.Business.Interfaces;
using MINT.EShop.Core.Entities.Product;
using MINT.EShop.Core.Interfaces;

namespace MINT.EShop.Business.Services
{
    public class CategoryService(IUnitOfWork unitOfWork) : ICategoryService
    {
        public async Task<CategoryResponse> CreateAsync(CreateCategoryRequest request)
        {
            // Створюємо категорію
            var category = new Category
            {
                Name = request.Name,
                Slug = request.Slug
            };

            // Зберігаємо категорію у репозиторії
            await unitOfWork.Categories.AddAsync(category);

            // Завершуємо транзакцію, щоб зберегти зміни в базі даних
            await unitOfWork.CompleteAsync();

            // Повертаємо відповідь у форматі CategoryResponse
            return new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Slug = category.Slug
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            // Отримуємо існуючу категорію за ID та перевіряємо на null
            var existingCategory = await unitOfWork.Categories.GetByIdAsync(id);
            if (existingCategory == null) return false;

            // Видаляємо категорію з репозиторію
            unitOfWork.Categories.Delete(existingCategory);

            // Завершуємо транзакцію, щоб зберегти зміни в базі даних
            await unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllAsync()
        {
            // Отримуємо список всіх категорій з репозиторію
            var categories = await unitOfWork.Categories.GetAllAsync();

            // Повертаємо послідовність категорій у форматі CategoryResponse
            return categories.Select(c => new CategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug
            });
        }

        public async Task<CategoryResponse?> GetByIdAsync(Guid id)
        {
            // Отримуємо категорію за ID
            var category = await unitOfWork.Categories.GetByIdAsync(id);

            // Якщо категорію не знайдено, повертаємо null
            if (category == null) return null;

            // Повертаємо відповідь у форматі CategoryResponse
            return new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Slug = category.Slug
            };
        }

        public async Task<CategoryResponse?> UpdateAsync(Guid id, UpdateCategoryRequest request)
        {
            // Отримуємо існуючу категорію за ID та перевіряємо на null
            var existingCategory = await unitOfWork.Categories.GetByIdAsync(id);
            if (existingCategory == null)
                return null;

            // Оновлюємо властивості категорії на основі даних з запиту
            existingCategory.Name = request.Name;
            existingCategory.Slug = request.Slug;

            // Завершуємо транзакцію, щоб зберегти зміни в базі даних
            await unitOfWork.CompleteAsync();

            // Повертаємо оновлену категорію у форматі CategoryResponse
            return new CategoryResponse()
            {
                Id = existingCategory.Id,
                Name = existingCategory.Name,
                Slug = existingCategory.Slug
            };
        }
    }
}
