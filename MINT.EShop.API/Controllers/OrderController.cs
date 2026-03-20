using Microsoft.AspNetCore.Mvc;
using MINT.EShop.API.Wrappers;
using MINT.EShop.Business.DTOs.OrderItems;
using MINT.EShop.Business.DTOs.Orders;
using MINT.EShop.Business.Interfaces;
using MINT.EShop.Core.Enums;

namespace MINT.EShop.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class OrderController(IOrderService orderService) : ControllerBase
    {
        /// <summary>
        /// Отримати інформацію про всі замовлення.
        /// </summary>
        /// <returns>Повертає послідовність всіх замовлень у форматі APIResponse.</returns>
        /// <response code="200">Успішно виконана операція.</response>
        [HttpGet]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<OrderResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            // Отримуємо результат бізнес-логіки
            var orders = await orderService.GetAllAsync();

            // Формуємо відповідь у форматі ApiResponse
            var response = APIResponse<IEnumerable<OrderResponse>>.SuccessResponse(orders);

            // Надсилаємо відповідь
            return Ok(response);
        }

        /// <summary>
        /// Отримати інформацію про замовлення за його унікальним ідентифікатором.
        /// </summary>
        /// <param name="id">Унікальний ідентифікатор товару (GUID).</param>
        /// <returns>Повертає інформацію про замовлення у форматі APIResponse.</returns>
        /// <response code="200">Замовлення успішно знайдено.</response>
        /// <response code="404">Немає замовлення за вказаним ID.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(APIResponse<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            // Отримуємо результат бізнес-логіки
            var order = await orderService.GetByIdAsync(id);
            if (order == null) return NotFound(APIResponse.FailureResponse("Order not found"));

            // Формуємо відповідь у форматі ApiResponse
            var response = APIResponse<OrderResponse>.SuccessResponse(order);

            // Надсилаємо відповідь
            return Ok(response);
        }

        /// <summary>
        /// Створити нове замовлення на основі наданих даних.
        /// </summary>
        /// <param name="request">CreateOrderRequest (поля: ClientId, OrderItems).</param>
        /// <returns>Повертає створене замовлення у форматі APIResponse.</returns>
        /// <response code="201">Замовлення успішно створено.</response>
        /// <response code="400">Некоректні дані для створення замовлення.</response>
        [HttpPost]
        [ProducesResponseType(typeof(APIResponse<OrderResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
        {
            // Викликаємо бізнес-логіку для створення замовлення
            var order = await orderService.CreateAsync(request);

            // Формуємо відповідь у форматі ApiResponse
            var response = APIResponse<OrderResponse>.SuccessResponse(order);

            // Надсилаємо відповідь.
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, response);
        }
        /// <summary>
        /// Оновити статус замовлення.
        /// </summary>
        /// <param name="id">Унікальний ідентифікатор замовлення (GUID).</param>
        /// <param name="request">Новий статус замовлення (Cancelled, Pending, Paid, Shipped, Delivered).</param>
        /// <returns>Повертає оновлену інформацію про замовлення у форматі APIResponse.</returns>
        /// <response code="200">Статус замовлення успішно оновлено.</response>
        /// <response code="400">Некоректні дані для оновлення статусу замовлення.</response>
        /// <response code="404">Немає замовлення за вказаним ID.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(APIResponse<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateOrderStatusRequest request)
        {
            // Викликаємо бізнес-логіку для оновлення статусу замовлення з перевіркою існування замовлення
            bool isUpdated = await orderService.UpdateStatusAsync(id, request.OrderStatus);
            if (isUpdated == false) return NotFound(APIResponse.FailureResponse("Order not found"));

            // Отримуємо оновлену інформацію про замовлення
            var order = await orderService.GetByIdAsync(id);

            // Формуємо відповідь у форматі ApiResponse
            var response = APIResponse<OrderResponse>.SuccessResponse(order, "Order status updated successfully.");

            // Надсилаємо відповідь
            return Ok(response);
        }

        /// <summary>
        /// Збільшити кількість товару певної позиції в замовленні.
        /// </summary>
        /// <param name="orderId">Унікальний ідентифікатор замовлення (GUID).</param>
        /// <param name="orderItemId">Унікальний ідентифікатор товару в замовленні (GUID).</param>
        /// <returns>Повертає оновлену інформацію про замовлення у форматі APIResponse.</returns>
        /// <response code="200">Кількість товару в замовленні успішно збільшено.</response>
        /// <response code="400">Некоректні дані для збільшення кількості товару.</response>
        /// <response code="404">Немає замовлення за вказаним ID.</response>
        [HttpPost("{orderId}/items/{orderItemId}/increase")]
        [ProducesResponseType(typeof(APIResponse<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IncreaseOrderItemCount(Guid orderId, Guid orderItemId)
        {
            // Викликаємо бізнес-логіку для збільшення кількості товару в замовленні з перевіркою існування замовлення
            var result = await orderService.IncreaseOrderItemQuantityAsync(orderId, orderItemId);
            if (result == null) return NotFound(APIResponse.FailureResponse("Order not found"));

            // Формуємо відповідь у форматі ApiResponse
            var response = APIResponse<OrderResponse>.SuccessResponse(result, "Order item count increased successfully.");

            // Надсилаємо відповідь
            return Ok(response);
        }

        /// <summary>
        /// Зменшити кількість товару певної позиції в замовленні.
        /// </summary>
        /// <param name="orderId">Унікальний ідентифікатор замовлення (GUID).</param>
        /// <param name="orderItemId">Унікальний ідентифікатор товару в замовленні (GUID).</param>
        /// <returns>Повертає оновлену інформацію про замовлення у форматі APIResponse.</returns>
        /// <response code="200">Кількість товару в замовленні успішно зменшено.</response>
        /// <response code="400">Некоректні дані для зменшення кількості товару.</response>
        /// <response code="404">Немає замовлення за вказаним ID.</response>
        [HttpPost("{orderId}/items/{orderItemId}/decrease")]
        [ProducesResponseType(typeof(APIResponse<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DecreaseOrderItemCount(Guid orderId, Guid orderItemId)
        {
            // Викликаємо бізнес-логіку для зменшення кількості товару в замовленні з перевіркою існування замовлення
            var result = await orderService.DecreaseOrderItemQuantityAsync(orderId, orderItemId);
            if (result == null) return NotFound(APIResponse.FailureResponse("Order not found"));

            // Формуємо відповідь у форматі ApiResponse
            var response = APIResponse<OrderResponse>.SuccessResponse(result, "Order item count decreased successfully.");

            // Надсилаємо відповідь
            return Ok(response);
        }

        /// <summary>
        /// Додати нову позицію товару до замовлення.
        /// </summary>
        /// <param name="orderId">Унікальний ідентифікатор замовлення (GUID).</param>
        /// <param name="orderItemRequest">Дані для створення нової позиції товару в замовленні.</param>
        /// <returns>Повертає інформацію про оновлене замовлення у форматі APIResponse.</returns>
        /// <response code="200">Позицію товару в замовленні успішно додано.</response>
        /// <response code="404">Немає замовлення за вказаним ID.</response>
        /// <response code="400">Некоректні дані для створення позиції товару.</response>
        [HttpPost("{orderId}/items")]
        [ProducesResponseType(typeof(APIResponse<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddOrderItem(Guid orderId, [FromBody] CreateOrderItemRequest orderItemRequest)
        {
            // Викликаємо бізнес-логіку для додавання товару до замовлення з перевіркою існування замовлення
            var result = await orderService.AddOrderItemAsync(orderId, orderItemRequest);
            if (result == null) return NotFound(APIResponse.FailureResponse("Order not found"));

            // Формуємо відповідь у форматі ApiResponse
            var response = APIResponse<OrderResponse>.SuccessResponse(result, "Order item added successfully.");

            // Надсилаємо відповідь
            return Ok(response);
        }

        /// <summary>
        /// Видалити позицію товару з замовлення.
        /// </summary>
        /// <param name="orderId">Унікальний ідентифікатор замовлення (GUID).</param>
        /// <param name="orderItemId">Унікальний ідентифікатор позиції товару (GUID).</param>
        /// <returns>Повертає інформацію про оновлене замовлення у форматі APIResponse.</returns>
        /// <response code="200">Позицію товару в замовленні успішно видалено.</response>
        /// <response code="400">Некоректні дані для видалення позиції товару.</response>
        /// <response code="404">Немає замовлення за вказаним ID.</response>
        [HttpDelete("{orderId}/items/{orderItemId}")]
        [ProducesResponseType(typeof(APIResponse<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteOrderItem(Guid orderId, Guid orderItemId)
        {
            // Викликаємо бізнес-логіку для видалення товару з замовлення з перевіркою існування замовлення
            var isDeleted = await orderService.DeleteOrderItemAsync(orderId, orderItemId);
            if (isDeleted == false) return NotFound(APIResponse.FailureResponse("Order not found"));

            // Отримуємо оновлену інформацію про замовлення
            var order = await orderService.GetByIdAsync(orderId);

            // Формуємо відповідь у форматі ApiResponse
            var response = APIResponse<OrderResponse>.SuccessResponse(order, "Order item deleted successfully.");

            // Надсилаємо відповідь
            return Ok(response);
        }
    }
}