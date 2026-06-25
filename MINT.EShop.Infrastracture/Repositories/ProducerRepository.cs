using Microsoft.EntityFrameworkCore;
using MINT.EShop.Core.Entities.Product;
using MINT.EShop.Core.Interfaces;

namespace MINT.EShop.Infrastracture.Repositories
{
    public class ProducerRepository(AppDbContext dbContext) : IProducerRepository
    {
        public async Task AddAsync(Producer producer)
        {
            await dbContext.Producers.AddAsync(producer);
        }

        public void Delete(Producer producer)
        {
            dbContext.Producers.Remove(producer);
        }

        public async Task<IEnumerable<Producer>> GetAllAsync()
        {
            return await dbContext.Producers.ToListAsync();
        }

        public async Task<Producer?> GetByIdAsync(Guid producerId)
        {
            return await dbContext.Producers.FirstOrDefaultAsync(p => p.Id == producerId);
        }

        public void Update(Producer producer)
        {
            dbContext.Producers.Update(producer);
        }
    }
}
