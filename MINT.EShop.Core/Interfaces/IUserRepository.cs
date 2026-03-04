using MINT.EShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid userId);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        void Update(User user);
        void Delete(User user);

    }
}
