using MINT.EShop.Business.DTOs.OrderItems;
using MINT.EShop.Business.DTOs.Orders;
using MINT.EShop.Business.Interfaces;
using MINT.EShop.Core.Entities.Order;
using MINT.EShop.Core.Enums;
using MINT.EShop.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Business.Services
{
    public class OrderService(IUnitOfWork unitOfWork) : IOrderService
    {
        public async Task<OrderResponse> CreateAsync(CreateOrderRequest request)
        {
            // TODO: Додати методи зміни параметрів замовлення для адміністраторів
            // TODO: Додати логіку знижок

            // Отримуємо унікальні ID продуктів з позицій замовлення та завантажуємо їх з бази даних
            var productsIDs = request.OrderItems.Select(item => item.ProductId).Distinct();
            var products = await unitOfWork.Products.GetByIdsAsync(productsIDs);

            // Створюємо словник для швидкого доступу до продуктів за їх ID
            var productsDict = products.ToDictionary(p => p.Id);

            // Створюємо нове замовлення
            var order = new Order
            {
                Id = Guid.NewGuid(),
                ClientId = request.ClientId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = 0,
                Status = OrderStatus.Pending
            };

            // Додаємо позиції замовлення та обчислюємо загальну суму замовлення
            foreach (var item in request.OrderItems)
            {
                // Перевіряємо, чи існує продукт з вказаним ID
                if (!productsDict.TryGetValue(item.ProductId, out var product))
                {
                    throw new KeyNotFoundException($"Product with ID {item.ProductId} not found.");
                }

                var orderItem = new OrderItem
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price,
                };

                order.OrderItems.Add(orderItem);

                order.TotalAmount += orderItem.UnitPrice * orderItem.Quantity;
            }

            // Додаємо замовлення до бази даних
            await unitOfWork.Orders.AddAsync(order);

            // Завершуємо транзакцію, щоб зберегти замовлення та його позиції в базі даних
            await unitOfWork.CompleteAsync();

            // Повертаємо відповідь з даними створеного замовлення
            return new OrderResponse
            {
                Id = order.Id,
                ClientId = order.ClientId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                OrderItems = [.. order.OrderItems.Select(oi => new OrderItemResponse
                {
                    Id = oi.Id,
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    Discount = oi.Discount
                })]
            };
        }

        public async Task<IEnumerable<OrderResponse>> GetAllAsync()
        {
            // Отримуємо всі замовлення з бази даних
            var orders = await unitOfWork.Orders.GetAllAsync();

            // Формуємо та повертаємо послідовність відповідей з даними замовлень
            return orders.Select(order => new OrderResponse
            {
                Id = order.Id,
                ClientId = order.ClientId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                OrderItems = [.. order.OrderItems.Select(oi => new OrderItemResponse
                {
                    Id = oi.Id,
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    Discount = oi.Discount
                })]
            });
        }

        public async Task<OrderResponse?> GetByIdAsync(Guid id)
        {
            // Отримуємо замовлення за ID з бази даних
            var order = await unitOfWork.Orders.GetByIdAsync(id);

            // Якщо замовлення не знайдено, повертаємо null
            if (order == null) return null;

            // Формуємо та повертаємо відповідь з даними замовлення
            return new OrderResponse
            {
                Id = order.Id,
                ClientId = order.ClientId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                OrderItems = [.. order.OrderItems.Select(oi => new OrderItemResponse
                {
                    Id = oi.Id,
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    Discount = oi.Discount
                })]
            };
        }

        public async Task<bool> UpdateStatusAsync(Guid id, OrderStatus newStatus)
        {
            // Отримуємо замовлення за ID та перевіряємо на null
            var order = await unitOfWork.Orders.GetByIdAsync(id);
            if (order == null) return false;

            // Змінюємо статус замовлення
            order.Status = newStatus;

            //// Повідомляємо EF про зміни (Необов'язкова дія)
            unitOfWork.Orders.Update(order);

            // Завершуємо транзакцію, щоб зберегти зміни в базі даних
            await unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<OrderResponse> IncreaseOrderItem
    }
}
