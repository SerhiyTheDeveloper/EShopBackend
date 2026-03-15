using Microsoft.EntityFrameworkCore;
using MINT.EShop.Core.Entities.Order;
using MINT.EShop.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Infrastracture.Repositories
{
    public class OrderRepository(AppDbContext dbContext) : IOrderRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task AddAsync(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
        }

        public void Delete(Order order)
        {
            _dbContext.Orders.Remove(order);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _dbContext.Orders
                .Include(o => o.OrderItems)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(Guid orderId)
        {
            return await _dbContext.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public void Update(Order order)
        {
            _dbContext.Orders.Update(order);
        }

        public void AddOrderItem(OrderItem item)
        {
            _dbContext.Set<OrderItem>().Add(item);
        }
    }
}