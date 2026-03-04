using MINT.EShop.Business.DTOs.OrderItems;
using MINT.EShop.Business.DTOs.Orders;
using MINT.EShop.Business.Interfaces;
using MINT.EShop.Core.Entities;
using MINT.EShop.Core.Entities.Order;
using MINT.EShop.Core.Enums;
using MINT.EShop.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            // TODO: Додати логіку знижок

            // Отримуємо унікальні ID продуктів з позицій замовлення та завантажуємо їх з бази даних
            var productsIds = request.OrderItems.Select(oi => oi.ProductId).Distinct();
            var products = await unitOfWork.Products.GetByIdsAsync(productsIds);

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
            // Отримуємо замовлення за ID та перевіряємо його на null
            var order = await unitOfWork.Orders.GetByIdAsync(id);
            if (order == null) return false;

            // Змінюємо статус замовлення
            order.Status = newStatus;

            // Завершуємо транзакцію, щоб зберегти зміни в базі даних
            await unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<OrderResponse?> IncreaseOrderItemCountAsync(Guid orderId, Guid orderItemId)
        {
            // Отримуємо замовлення та перевіряємо його на null
            var order = await unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null) return null;

            // Дістаємо та перевіряємо наявність позиції замовелення
            var orderItem = order.OrderItems.FirstOrDefault(oi => oi.Id == orderItemId)
                ?? throw new KeyNotFoundException($"Order item with ID {orderItemId} was not found in the order {orderId}");

            // Збільшуємо кількість позиції замовлення
            orderItem.Quantity++;

            // Збільшуємо загальну ціну замовлення
            order.TotalAmount += orderItem.UnitPrice;

            // Завершуємо транзакцію, щоб зберегти зміни в базі даних
            await unitOfWork.CompleteAsync();

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

        public async Task<OrderResponse?> DecreaseOrderItemCountAsync(Guid orderId, Guid orderItemId)
        {
            // Отримуємо замовлення та перевіряємо його на null
            var order = await unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null) return null;

            // Дістаємо та перевіряємо наявність позиції замовлення
            var orderItem = order.OrderItems.FirstOrDefault(oi => oi.Id == orderItemId)
                ?? throw new KeyNotFoundException($"Order item with ID {orderItemId} was not found in the order {orderId}");

            // Зменшуємо кількість позиції замовлення
            orderItem.Quantity--;

            // Зменшуємо загальну ціну замовлення
            order.TotalAmount -= orderItem.UnitPrice;

            // Завершуємо транзакцію, щоб зберегти зміни в базі даних
            await unitOfWork.CompleteAsync();

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

        public async Task<OrderResponse?> AddOrderItemAsync(Guid orderId, CreateOrderItemRequest orderItemRequest)
        {
            // Отримуємо замовлення та перевіряємо його на null
            var order = await unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null) return null;

            // Отримуємо продукт та перевіряємо його на null
            var product = await unitOfWork.Products.GetByIdAsync(orderItemRequest.ProductId)
                ?? throw new KeyNotFoundException($"Product with ID {orderItemRequest.ProductId} not found.");

            // Формуємо позицію замовлення
            var orderItem = new OrderItem 
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                ProductId = orderItemRequest.ProductId,
                Quantity = orderItemRequest.Quantity,
                UnitPrice = product.Price,
            };

            // Додаємо позицію замовлення в базу даних
            order.OrderItems.Add(orderItem);

            // Збільшуємо загальну суму замовлення
            order.TotalAmount += orderItem.UnitPrice * orderItem.Quantity;

            // Завершуємо транзакцію, щоб зберегти зміни в базі даних
            await unitOfWork.CompleteAsync();

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

        public async Task<bool> DeleteOrderItemAsync(Guid orderId, Guid orderItemId)
        {
            // Отримуємо замовлення та перевіряємо його на null
            var order = await unitOfWork.Orders.GetByIdAsync(orderItemId);
            if (order == null) return false;

            // Дістаємо та перевіряємо наявність позиції замовлення
            var orderItem = order.OrderItems.FirstOrDefault(oi => oi.Id == orderItemId)
                ?? throw new KeyNotFoundException($"Order Item with ID  {orderItemId}  was not found in the order {orderId}");

            // Зменшуємо загальну суму замовлення
            order.TotalAmount -= orderItem.UnitPrice * orderItem.Quantity;

            // Видаляємо позицію замовлення з бази даних
            order.OrderItems.Remove(orderItem);

            // Завершуємо транзакцію, щоб зберегти зміни в базі даних
            await unitOfWork.CompleteAsync();

            // Повертаємо відповідь про успішне завершення операції
            return true;
        }
    }
}