using MINT.EShop.Core.Entities.UserData;

namespace MINT.EShop.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid userId);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> ExistsByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        void AddSession(UserSession session);
        void Update(User user);
        void Delete(User user);

    }
}
