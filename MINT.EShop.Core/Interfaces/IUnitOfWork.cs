namespace MINT.EShop.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable 
    {
        public IUserRepository Users { get; }
        public IProductRepository Products { get; }
        public IOrderRepository Orders { get; }
        public ICategoryRepository Categories { get; }
        public IProducerRepository Producers { get; }
        public IWishlistItemRepository WishlistItems { get; }
        Task<int> CompleteAsync();
    }
}
