using MINT.EShop.Core.Entities.Order;
using MINT.EShop.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Infrastracture.Repositories
{
    public class InMemoryOrderItemRepository : IOrderItemRepository
    {
        private readonly List<OrderItem> _orderItems = [];

        public Task AddAsync(OrderItem orderItem)
        {
            _orderItems.Add(orderItem);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid orderItemId)
        {
            _orderItems.RemoveAll(oi => oi.Id == orderItemId);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<OrderItem>> GetAllAsync() =>
            Task.FromResult<IEnumerable<OrderItem>>(_orderItems);

        public Task<OrderItem?> GetByIdAsync(Guid orderItemId) =>
            Task.FromResult(_orderItems.FirstOrDefault(oi => oi.Id == orderItemId));

        public Task UpdateAsync(OrderItem orderItem) =>
            Task.CompletedTask;
    }
}
