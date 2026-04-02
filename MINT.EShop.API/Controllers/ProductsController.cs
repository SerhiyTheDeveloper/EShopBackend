using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MINT.EShop.API.Wrappers;
using MINT.EShop.Business.DTOs.Products;
using MINT.EShop.Business.Interfaces;
using MINT.EShop.Core.Entities;
using System.Net.WebSockets;

namespace MINT.EShop.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class ProductsController(IProductService productService, ILogger<ProductsController> logger) : BaseController
    {
        /// <summary>
        /// Отримати інформацію про всі товари, доступні в магазині.
        /// </summary>
        /// <returns>Повертає послідовність всіх товарів у форматі APIResponse.</returns>
        /// <response code="200">Успішно виконана операція.</response>
        [HttpGet]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<ProductResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] GetProductsFilter filter)
        {
            // Отримуємо результат бізнес-логіки
            var products = await productService.GetAllAsync(filter);

            // Формуємо відповідь у форматі ApiResponse та надсилаємо її
            var response = APIResponse<IEnumerable<ProductResponse>>.SuccessResponse(products);
            return Ok(response);
        }

        /// <summary>
        /// Отримати інформацію про товар за його ідентифікатором.
        /// </summary>
        /// <param name="id">Унікальний ідентифікатор товару (GUID).</param>
        /// <returns>Повертає інформацію про товар у форматі APIResponse.</returns>
        /// <response code="200">Товар успішно знайдено.</response>
        /// <response code="404">Товар з таким ID не існує в базі.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(APIResponse<ProductResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetById(Guid id)
        {
            // Отримуємо результат бізнес-логіки та перевіряємо його на null
            var product = await productService.GetByIdAsync(id);
            if (product == null) return NotFound(APIResponse.FailureResponse("Product not found"));

            // Формуємо відповідь у форматі ApiResponse
            var response = APIResponse<ProductResponse>.SuccessResponse(product);

            // Надсилаємо відповідь
            return Ok(response);
        }

        /// <summary>
        /// Отримати інформацію про безліч товарів за їхніми ідентифікаторами.
        /// </summary>
        /// <param name="ids">Унікальні ідентифікатори товарів (GUID).</param>
        /// <returns>Повертає послідовність товарів у форматі APIResponse.</returns>
        /// <response code="200">Операція успішно виконана.</response>
        [HttpGet("by-ids")]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<ProductResponse>>), StatusCodes.Status200OK)]

        public async Task<IActionResult> GetByIds([FromQuery] IEnumerable<Guid> ids)
        {
            // Отримуємо результат бізнес-логіки
            var products = await productService.GetByIdsAsync(ids);

            // Формуємо відповідь у форматі ApiResponse та надсилаємо її
            var response = APIResponse<IEnumerable<ProductResponse>>.SuccessResponse(products);
            return Ok(response);
        }

        /// <summary>
        /// Створити новий товар у магазині. (Manager)
        /// </summary>
        /// <param name="request">CreateProductRequest (поля: Name, Description, Price).</param>
        /// <returns>Повертає створений товар у форматі APIResponse.</returns>
        /// <response code="201">Товар успішно створено.</response>
        /// <response code="400">Некоректні дані для створення товару.</response>
        /// <response code="401">Користувач не авторизований.</response>
        /// <response code="403">Недостатньо прав.</response>
        [HttpPost]
        [Authorize("ManagerPolicy")]
        [ProducesResponseType(typeof(APIResponse<ProductResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            // Отримуємо результат бізнес-логіки
            var product = await productService.CreateAsync(CurrentUserId, request);

            // Формуємо відповідь у форматі ApiResponse
            var response = APIResponse<ProductResponse>.SuccessResponse(product);

            // Надсилаємо відповідь
            logger.LogInformation("Product created with ID {ProductId} by manager {UserId}", product.Id, CurrentUserId);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, response);
        }

        /// <summary>
        /// Оновити інформацію про існуючий товар за його ідентифікатором. (Manager)
        /// </summary>
        /// <param name="id">Унікальний ідентифікатор товару (GUID).</param>
        /// <param name="request">UpdateProductRequest (поля: Name, Description, Price).</param>
        /// <returns>Повертає оновлений товар у форматі APIResponse.</returns>
        /// <response code="200">Товар успішно оновлено.</response>
        /// <response code="404">Товар з таким ID не існує в базі.</response>
        /// <response code="401">Користувач не авторизований.</response>
        /// <response code="403">Недостатньо прав.</response>
        [HttpPut("{id}")]
        [Authorize("ManagerPolicy")]
        [ProducesResponseType(typeof(APIResponse<ProductResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest request)
        {
            // Отримуємо результат бізнес-логіки та перевіряємо його на null
            var updated = await productService.UpdateAsync(id, request);
            if (updated == null) return NotFound(APIResponse.FailureResponse("Product not found"));

            // Формуємо відповідь у форматі ApiResponse та надсилаємо її
            var response = APIResponse<ProductResponse>.SuccessResponse(updated);
            logger.LogInformation("Product with ID {ProductId} updated by manager {UserId}", id, CurrentUserId);
            return Ok(response);
        }

        /// <summary>
        /// Видалити товар з магазину за його ідентифікатором. (Manager)
        /// </summary>
        /// <param name="id">Унікальний ідентифікатор товару (GUID).</param>
        /// <returns>Повертає ID видаленого товару.</returns>
        /// <response code="200">Товар успішно видалено.</response>
        /// <response code="404">Товар з таким ID не існує в базі.</response>
        /// <response code="401">Користувач не авторизований.</response>
        /// <response code="403">Недостатньо прав.</response>
        [HttpDelete("{id}")]
        [Authorize("ManagerPolicy")]
        [ProducesResponseType(typeof(APIResponse<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete(Guid id)
        {
            // Отримуємо результат бізнес-логіки
            var isDeleted = await productService.DeleteAsync(id);

            // Перевіряємо результат та формуємо відповідь у форматі ApiResponse
            if (!isDeleted) return NotFound(APIResponse.FailureResponse($"Product with ID {id} not found"));
            logger.LogInformation("Product with ID {ProductId} deleted by manager {UserId}", id, CurrentUserId);
            return Ok(APIResponse<Guid>.SuccessResponse(id));
        }
    }
}