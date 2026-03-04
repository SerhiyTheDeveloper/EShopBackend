using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable 
    {
        public IUserRepository Users { get; }
        public IProductRepository Products { get; }
        public IOrderRepository Orders { get; }
        Task<int> CompleteAsync();
    }
}
