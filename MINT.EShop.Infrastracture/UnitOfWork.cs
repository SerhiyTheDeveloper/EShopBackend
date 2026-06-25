using MINT.EShop.Core.Interfaces;
using MINT.EShop.Infrastracture.Repositories;

namespace MINT.EShop.Infrastracture
{
    public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
    {
        private readonly AppDbContext _dbContext = dbContext;

        private IUserRepository? _userRepository;
        private IProductRepository? _productRepository;
        private IOrderRepository? _orderRepository;
        private ICategoryRepository? _categoryRepository;
        private IProducerRepository? _producerRepository;

        public IUserRepository Users => _userRepository ??= new UserRepository(_dbContext);
        public IProductRepository Products => _productRepository ??= new ProductRepository(_dbContext);
        public IOrderRepository Orders => _orderRepository ??= new OrderRepository(_dbContext);
        public ICategoryRepository Categories => _categoryRepository ??= new CategoryRepository(_dbContext);
        public IProducerRepository Producers => _producerRepository ??= new ProducerRepository(_dbContext);

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
