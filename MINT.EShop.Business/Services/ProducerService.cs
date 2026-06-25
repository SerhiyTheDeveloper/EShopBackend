using MINT.EShop.Business.DTOs.Category;
using MINT.EShop.Business.DTOs.Producers;
using MINT.EShop.Business.Interfaces;
using MINT.EShop.Core.Entities.Product;
using MINT.EShop.Core.Interfaces;

namespace MINT.EShop.Business.Services
{
    public class ProducerService(IUnitOfWork unitOfWork) : IProducerService
    {
        public async Task<ProducerResponse> CreateAsync(CreateProducerRequest request)
        {
            // Створюємо виробника
            var producer = new Producer
            {
                Name = request.Name,
                Slug = request.Slug
            };

            // Зберігаємо виробника у репозиторії
            await unitOfWork.Producers.AddAsync(producer);

            // Завершуємо транзакцію, щоб зберегти зміни в базі даних
            await unitOfWork.CompleteAsync();

            // Повертаємо відповідь у форматі ProducerResponse
            return new ProducerResponse
            {
                Id = producer.Id,
                Name = producer.Name,
                Slug = producer.Slug
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            // Отримуємо існуючого виробника за ID та перевіряємо на null
            var existingProducer = await unitOfWork.Producers.GetByIdAsync(id);
            if (existingProducer == null) return false;

            // Видаляємо виробника з репозиторію
            unitOfWork.Producers.Delete(existingProducer);

            // Завершуємо транзакцію, щоб зберегти зміни в базі даних
            await unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<IEnumerable<ProducerResponse>> GetAllAsync()
        {
            // Отримуємо список всіх виробників з репозиторію
            var producers = await unitOfWork.Producers.GetAllAsync();

            // Повертаємо послідовність виробників у форматі ProducerResponse
            return producers.Select(p => new ProducerResponse
            {
                Id = p.Id,
                Name = p.Name,
                Slug = p.Slug
            });
        }

        public async Task<ProducerResponse?> GetByIdAsync(Guid id)
        {
            // Отримуємо виробника за ID
            var producer = await unitOfWork.Producers.GetByIdAsync(id);

            // Якщо виробника не знайдено, повертаємо null
            if (producer == null) return null;

            // Повертаємо відповідь у форматі ProducerResponse
            return new ProducerResponse
            {
                Id = producer.Id,
                Name = producer.Name,
                Slug = producer.Slug
            };
        }

        public async Task<ProducerResponse?> UpdateAsync(Guid id, UpdateProducerRequest request)
        {
            // Отримуємо існуючого виробника за ID та перевіряємо на null
            var existingProducer = await unitOfWork.Producers.GetByIdAsync(id);
            if (existingProducer == null)
                return null;

            // Оновлюємо властивості виробника на основі даних з запиту
            existingProducer.Name = request.Name;
            existingProducer.Slug = request.Slug;

            // Завершуємо транзакцію, щоб зберегти зміни в базі даних
            await unitOfWork.CompleteAsync();

            // Повертаємо оновленого виробника у форматі ProducerResponse
            return new ProducerResponse()
            {
                Id = existingProducer.Id,
                Name = existingProducer.Name,
                Slug = existingProducer.Slug
            };
        }
    }
}
