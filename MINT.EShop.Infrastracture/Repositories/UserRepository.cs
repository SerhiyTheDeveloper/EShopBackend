using Microsoft.EntityFrameworkCore;
using MINT.EShop.Core.Entities.UserData;
using MINT.EShop.Core.Interfaces;

namespace MINT.EShop.Infrastracture.Repositories
{
    public class UserRepository(AppDbContext dbContext) : IUserRepository
    {
        public async Task AddAsync(User user)
        {
            await dbContext.Users.AddAsync(user);
        }

        public void Delete(User user)
        {
            dbContext.Users.Remove(user);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await dbContext.Users
                .Include(u => u.Credential)
                .Include(u => u.Sessions)
                .Include(u => u.ClientAccount)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(Guid userId)
        {
            return await dbContext.Users
                .Include(u => u.Credential)
                .Include(u => u.Sessions)
                .Include (u => u.ClientAccount)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await dbContext.Users
                .Include(u => u.Credential)
                .Include(u => u.Sessions)
                .Include(u => u.ClientAccount)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await dbContext.Users.AnyAsync(u => u.Email == email);
        }

        public void AddSession(UserSession session)
        {
            dbContext.Set<UserSession>().Add(session);
        }

        public void Update(User user)
        {
            dbContext.Users.Update(user);
        }
    }
}
