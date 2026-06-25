using MINT.EShop.Core.Entities.Product;

namespace MINT.EShop.Core.Interfaces
{
    public interface IProducerRepository
    {
        Task<Producer?> GetByIdAsync(Guid producerId);
        Task<IEnumerable<Producer>> GetAllAsync();
        Task AddAsync(Producer producer);
        void Update(Producer producer);
        void Delete(Producer producer);
    }
}
