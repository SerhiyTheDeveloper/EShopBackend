using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MINT.EShop.API.Wrappers;
using MINT.EShop.Business.DTOs.OrderItems;
using MINT.EShop.Business.DTOs.Orders;
using MINT.EShop.Business.Interfaces;

namespace MINT.EShop.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    [Produces("application/json")]
    public class OrderController(IOrderService orderService, ILogger<OrderController> logger) : BaseController
    {
        /// <summary>
        /// Отримати інформацію про всі замовлення. (Manager)
        /// </summary>
        /// <param name="clientAccountId">Унікальний ідентифікатор клієнта (за потреби).</param>
        /// <param name="filter">Фільтр для отримання замовлень (поля: Status).</param>
        /// <returns>Послідовність всіх замовлень у форматі APIResponse.</returns>
        /// <response code="200">Успішно виконана операція.</response>
        /// <response code="401">Користувач не авторизований.</response>
        /// <response code="403">Недостатньо прав.</response>
        [HttpGet]
        [Authorize("ManagerPolicy")]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<OrderResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAll([FromQuery] Guid? clientAccountId, [FromQuery] GetOrdersFilter filter)
        {
            // Отримуємо результат бізнес-логіки
            var orders = await orderService.GetAllAsync(clientAccountId, filter);

            // Формуємо відповідь у форматі ApiResponse
            var response = APIResponse<IEnumerable<OrderResponse>>.SuccessResponse(orders);

            // Надсилаємо відповідь
            logger.LogInformation("List of orders requested by manager {userId}", CurrentUserId);
            return Ok(response);
        }

        /// <summary>
        /// Отримати інформацію про всі замовлення поточного користувача.
        /// </summary>
        /// <returns>Послідовність всіх замовлень у форматі APIResponse.</returns>
        /// <response code="200">Успішно виконана операція.</response>
        /// <response code="401">Користувач не авторизований.</response>
        [HttpGet("my")]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<OrderResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMyOrders([FromQuery] GetOrdersFilter filter)
        { 
            // Отримуємо результат бізнес-логіки
            var orders = await orderService.GetAllAsync(CurrentClientAccountId, filter);

            // Формуємо відповідь у форматі ApiResponse
            var response = APIResponse<IEnumerable<OrderResponse>>.SuccessResponse(orders);

            // Надсилаємо відповідь
            return Ok(response);
        }

        /// <summary>
        /// Отримати інформацію про конкретне замовлення поточного користувача.
        /// </summary>
        /// <returns>Замовлення у форматі APIResponse.</returns>
        /// <response code="200">Успішно виконана операція.</response>
        /// <response code="401">Користувач не авторизований.</response>
        /// <response code="404">Немає замовлення за вказаним ID.</response>
        [HttpGet("my/{id}")]
        [ProducesResponseType(typeof(APIResponse<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMyById(Guid id)
        {
            // Отримуємо результат бізнес-логіки та перевіряємо на null
            var order = await orderService.GetByIdAsync(CurrentClientAccountId, id, CurrentUserRole);
            if (order == null) return NotFound(APIResponse.FailureResponse("Order not found"));

            // Формуємо відповідь у форматі ApiResponse
            var response = APIResponse<OrderResponse>.SuccessResponse(order);

            // Надсилаємо відповідь
            return Ok(response);
        }

        /// <summary>
        /// Отримати інформацію про замовлення за його унікальним ідентифікатором. (Manager)
        /// </summary>
        /// <param name="id">Унікальний ідентифікатор товару (GUID).</param>
        /// <returns>Інформацію про замовлення у форматі APIResponse.</returns>
        /// <response code="200">Замовлення успішно знайдено.</response>
        /// <response code="404">Немає замовлення за вказаним ID.</response>
        /// <response code="401">Користувач не авторизований.</response>
        /// <response code="403">Недостатньо прав.</response>
        [HttpGet("{id}")]
        [Authorize("ManagerPolicy")]
        [ProducesResponseType(typeof(APIResponse<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetById(Guid id)
        {
            // Отримуємо результат бізнес-логіки та перевіряємо на null
            var order = await orderService.GetByIdAsync(CurrentClientAccountId, id, CurrentUserRole);
            if (order == null) return NotFound(APIResponse.FailureResponse("Order not found"));

            // Формуємо відповідь у форматі ApiResponse
            var response = APIResponse<OrderResponse>.SuccessResponse(order);

            // Надсилаємо відповідь
            return Ok(response);
        }

        /// <summary>
        /// Створити нове замовлення на основі наданих даних.
        /// </summary>
        /// <param name="request">CreateOrderRequest (поля: OrderItems).</param>
        /// <returns>Створене замовлення у форматі APIResponse.</returns>
        /// <response code="201">Замовлення успішно створено.</response>
        /// <response code="400">Некоректні дані для створення замовлення.</response>
        /// <response code="401">Користувач не авторизований.</response>
        [HttpPost]
        [ProducesResponseType(typeof(APIResponse<OrderResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
        {
            // Викликаємо бізнес-логіку для створення замовлення
            var order = await orderService.CreateAsync(CurrentUserId, request);

            // Формуємо відповідь у форматі ApiResponse
            var response = APIResponse<OrderResponse>.SuccessResponse(order);

            // Надсилаємо відповідь.
            logger.LogInformation("Order {orderId} created by user {userId}", order.Id, CurrentUserId);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, response);
        }

        /// <summary>
        /// Оновити статус замовлення. (Manager)
        /// </summary>
        /// <param name="id">Унікальний ідентифікатор замовлення (GUID).</param>
        /// <param name="request">Новий статус замовлення (Cancelled, Pending, Paid, Shipped, Delivered).</param>
        /// <returns>Оновлену інформацію про замовлення у форматі APIResponse.</returns>
        /// <response code="200">Статус замовлення успішно оновлено.</response>
        /// <response code="400">Некоректні дані для оновлення статусу замовлення.</response>
        /// <response code="404">Немає замовлення за вказаним ID.</response>
        /// <response code="403">Недостатньо прав.</response>
        /// <response code="401">Користувач не авторизований.</response>
        [HttpPut("{id}")]
        [Authorize("ManagerPolicy")]
        [ProducesResponseType(typeof(APIResponse<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateOrderStatusRequest request)
        {
            // Викликаємо бізнес-логіку для оновлення статусу замовлення з перевіркою існування замовлення
            bool isUpdated = await orderService.UpdateStatusAsync(id, request.OrderStatus);
            if (isUpdated == false) return NotFound(APIResponse.FailureResponse("Order not found"));

            // Отримуємо оновлену інформацію про замовлення
            var order = await orderService.GetByIdAsync(CurrentClientAccountId, id, CurrentUserRole);

            // Формуємо відповідь у форматі ApiResponse
            var response = APIResponse<OrderResponse>.SuccessResponse(order, "Order status updated successfully.");

            // Надсилаємо відповідь
            logger.LogInformation("Order {orderId} status updated to {status} by manager {userId}", id, request.OrderStatus, CurrentUserId);
            return Ok(response);
        }

        /// <summary>
        /// Скасувати замовлення поточного користувача. Користувач може скасувати замовлення, якщо воно ще не оплачено.
        /// </summary>
        /// <param name="id">Унікальний ідентифікатор замовлення (GUID).</param>
        /// <returns>Оновлену інформацію про замовлення у форматі APIResponse.</returns>
        /// <response code="404">Немає замовлення за вказаним ID.</response>
        /// <response code="401">Користувач не авторизований.</response>
        /// <response code="400">Некоректні дані для скасування замовлення.</response>
        [HttpPut("my/{id}/cancel")]
        [ProducesResponseType(typeof(APIResponse<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CancelMyOrder(Guid id)
        {
            // Викликаємо бізнес-логіку для скасування замовлення з перевіркою існування замовлення та прав користувача
            bool isCancelled = await orderService.CancelMyAsync(CurrentUserId, id);
            if (isCancelled == false) return NotFound(APIResponse.FailureResponse("Order not found"));

            // Отримуємо оновлену інформацію про замовлення
            var order = await orderService.GetByIdAsync(CurrentClientAccountId, id, CurrentUserRole);

            // Формуємо відповідь у форматі ApiResponse
            var response = APIResponse<OrderResponse>.SuccessResponse(order, "Order cancelled successfully.");

            // Надсилаємо відповідь
            logger.LogInformation("Order {orderId} cancelled by user {userId}", id, CurrentUserId);
            return Ok(response);
        }

        /// <summary>
        /// Збільшити кількість товару певної позиції в замовленні. (Manager)
        /// </summary>
        /// <param name="orderId">Унікальний ідентифікатор замовлення (GUID).</param>
        /// <param name="orderItemId">Унікальний ідентифікатор товару в замовленні (GUID).</param>
        /// <returns>Оновлену інформацію про замовлення у форматі APIResponse.</returns>
        /// <response code="200">Кількість товару в замовленні успішно збільшено.</response>
        /// <response code="400">Некоректні дані для збільшення кількості товару.</response>
        /// <response code="404">Немає замовлення за вказаним ID.</response>
        /// <response code="403">Недостатньо прав.</response>
        /// <response code="401">Користувач не авторизований.</response>
        [HttpPost("{orderId}/items/{orderItemId}/increase")]
        [Authorize("ManagerPolicy")]
        [ProducesResponseType(typeof(APIResponse<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> IncreaseOrderItemCount(Guid orderId, Guid orderItemId)
        {
            // Викликаємо бізнес-логіку для збільшення кількості товару в замовленні з перевіркою існування замовлення
            var result = await orderService.IncreaseOrderItemQuantityAsync(orderId, orderItemId);
            if (result == null) return NotFound(APIResponse.FailureResponse("Order not found"));

            // Формуємо відповідь у форматі ApiResponse
            var response = APIResponse<OrderResponse>.SuccessResponse(result, "Order item count increased successfully.");

            // Надсилаємо відповідь
            logger.LogInformation("Order {orderId} item {orderItemId} count increased by manager {userId}", orderId, orderItemId, CurrentUserId);
            return Ok(response);
        }

        /// <summary>
        /// Зменшити кількість товару певної позиції в замовленні. (Manager)
        /// </summary>
        /// <param name="orderId">Унікальний ідентифікатор замовлення (GUID).</param>
        /// <param name="orderItemId">Унікальний ідентифікатор товару в замовленні (GUID).</param>
        /// <returns>Оновлену інформацію про замовлення у форматі APIResponse.</returns>
        /// <response code="200">Кількість товару в замовленні успішно зменшено.</response>
        /// <response code="400">Некоректні дані для зменшення кількості товару.</response>
        /// <response code="404">Немає замовлення за вказаним ID.</response>
        /// <response code="403">Недостатньо прав.</response>
        /// <response code="401">Користувач не авторизований.</response>
        [HttpPost("{orderId}/items/{orderItemId}/decrease")]
        [Authorize("ManagerPolicy")]
        [ProducesResponseType(typeof(APIResponse<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DecreaseOrderItemCount(Guid orderId, Guid orderItemId)
        {
            // Викликаємо бізнес-логіку для зменшення кількості товару в замовленні з перевіркою існування замовлення
            var result = await orderService.DecreaseOrderItemQuantityAsync(orderId, orderItemId);
            if (result == null) return NotFound(APIResponse.FailureResponse("Order not found"));

            // Формуємо відповідь у форматі ApiResponse
            var response = APIResponse<OrderResponse>.SuccessResponse(result, "Order item count decreased successfully.");

            // Надсилаємо відповідь
            logger.LogInformation("Order {orderId} item {orderItemId} count decreased by manager {userId}", orderId, orderItemId, CurrentUserId);
            return Ok(response);
        }

        /// <summary>
        /// Додати нову позицію товару до замовлення. (Manager)
        /// </summary>
        /// <param name="orderId">Унікальний ідентифікатор замовлення (GUID).</param>
        /// <param name="orderItemRequest">Дані для створення нової позиції товару в замовленні.</param>
        /// <returns>Інформацію про оновлене замовлення у форматі APIResponse.</returns>
        /// <response code="200">Позицію товару в замовленні успішно додано.</response>
        /// <response code="404">Немає замовлення за вказаним ID.</response>
        /// <response code="400">Некоректні дані для створення позиції товару.</response>
        /// <response code="403">Недостатньо прав.</response>
        /// <response code="401">Користувач не авторизований.</response>
        [HttpPost("{orderId}/items")]
        [Authorize("ManagerPolicy")]
        [ProducesResponseType(typeof(APIResponse<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AddOrderItem(Guid orderId, [FromBody] CreateOrderItemRequest orderItemRequest)
        {
            // Викликаємо бізнес-логіку для додавання товару до замовлення з перевіркою існування замовлення
            var result = await orderService.AddOrderItemAsync(orderId, orderItemRequest);
            if (result == null) return NotFound(APIResponse.FailureResponse("Order not found"));

            // Формуємо відповідь у форматі ApiResponse
            var response = APIResponse<OrderResponse>.SuccessResponse(result, "Order item added successfully.");

            // Надсилаємо відповідь
            logger.LogInformation("Order item with product {productId}, count {count} added to order {orderId} by manager {userId}",
                orderItemRequest.ProductId, orderItemRequest.Quantity, orderId, CurrentUserId);
            return Ok(response);
        }

        /// <summary>
        /// Видалити позицію товару з замовлення. (Manager)
        /// </summary>
        /// <param name="orderId">Унікальний ідентифікатор замовлення (GUID).</param>
        /// <param name="orderItemId">Унікальний ідентифікатор позиції товару (GUID).</param>
        /// <returns>Інформацію про оновлене замовлення у форматі APIResponse.</returns>
        /// <response code="200">Позицію товару в замовленні успішно видалено.</response>
        /// <response code="400">Некоректні дані для видалення позиції товару.</response>
        /// <response code="404">Немає замовлення за вказаним ID.</response>
        /// <response code="403">Недостатньо прав.</response>
        /// <response code="401">Користувач не авторизований.</response>
        [HttpDelete("{orderId}/items/{orderItemId}")]
        [Authorize("ManagerPolicy")]
        [ProducesResponseType(typeof(APIResponse<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteOrderItem(Guid orderId, Guid orderItemId)
        {
            // Викликаємо бізнес-логіку для видалення товару з замовлення з перевіркою існування замовлення
            var isDeleted = await orderService.DeleteOrderItemAsync(orderId, orderItemId);
            if (isDeleted == false) return NotFound(APIResponse.FailureResponse("Order not found"));

            // Отримуємо оновлену інформацію про замовлення
            var order = await orderService.GetByIdAsync(CurrentClientAccountId, orderId, CurrentUserRole);

            // Формуємо відповідь у форматі ApiResponse
            var response = APIResponse<OrderResponse>.SuccessResponse(order, "Order item deleted successfully.");

            // Надсилаємо відповідь
            logger.LogInformation("Order item {orderItemId} deleted from order {orderId} by manager {userId}", orderItemId, orderId, CurrentUserId);
            return Ok(response);
        }
    }
}