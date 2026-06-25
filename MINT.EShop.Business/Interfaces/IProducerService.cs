using MINT.EShop.Business.DTOs.Producers;

namespace MINT.EShop.Business.Interfaces
{
    public interface IProducerService
    {
        Task<IEnumerable<ProducerResponse>> GetAllAsync();
        Task<ProducerResponse?> GetByIdAsync(Guid id);
        Task<ProducerResponse> CreateAsync(CreateProducerRequest request);
        Task<ProducerResponse?> UpdateAsync(Guid id, UpdateProducerRequest request);
        Task<bool> DeleteAsync(Guid id);
    }
}
