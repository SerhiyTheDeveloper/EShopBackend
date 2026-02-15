using MINT.EShop.Core.Entities.Order;
using MINT.EShop.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Infrastracture.Repositories
{
    public class InMemoryOrderRepository : IOrderRepository
    {
        private readonly List<Order> _orders = [];

        public Task AddAsync(Order order)
        {
            _orders.Add(order);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid orderId) 
        {
            _orders.RemoveAll(o => o.Id == orderId);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Order>> GetAllAsync() =>
            Task.FromResult<IEnumerable<Order>>(_orders);

        public Task<Order?> GetByIdAsync(Guid orderId) =>
            Task.FromResult(_orders.FirstOrDefault(o => o.Id == orderId));

        public Task UpdateAsync(Order order) =>
            Task.CompletedTask;
    }
}
