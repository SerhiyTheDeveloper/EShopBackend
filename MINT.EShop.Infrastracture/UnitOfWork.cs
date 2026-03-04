using MINT.EShop.Core.Interfaces;
using MINT.EShop.Infrastracture.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Infrastracture
{
    public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
    {
        private readonly AppDbContext _dbContext = dbContext;

        private IUserRepository? _userRepository;
        private IProductRepository? _productRepository;
        private IOrderRepository? _orderRepository;

        public IUserRepository Users => _userRepository ??= new UserRepository(_dbContext);

        public IProductRepository Products => _productRepository ??= new ProductRepository(_dbContext);

        public IOrderRepository Orders => _orderRepository ??= new OrderRepository(_dbContext);

        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
