using MINT.EShop.Core.Entities;
using MINT.EShop.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Infrastracture.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users = [];

        public Task<User?> GetByIdAsync(Guid userId) =>
            Task.FromResult(_users.FirstOrDefault(u => u.Id == userId));

        public Task<IEnumerable<User>> GetAllAsync() =>       
            Task.FromResult<IEnumerable<User>>(_users); 

        public Task AddAsync(User user)
        {
            _users.Add(user);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(User user) =>
            Task.CompletedTask;

        public Task DeleteAsync(Guid userId)
        {
            _users.RemoveAll(u => u.Id == userId);
            return Task.CompletedTask;
        }
    }
}
