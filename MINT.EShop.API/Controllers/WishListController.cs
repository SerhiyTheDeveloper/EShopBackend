using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MINT.EShop.API.Wrappers;
using MINT.EShop.Business.DTOs.Products;
using MINT.EShop.Business.Interfaces;

namespace MINT.EShop.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    [Produces("application/json")]
    public class WishListController(IWishListService wishListService, ILogger<WishListController> logger) : BaseController
    {
        /// <summary>
        /// Отримати список вподобаних товарів поточного користувача.
        /// </summary>
        /// <returns>Послідовність товарів у форматі APIResponse.</returns>
        /// <response code="200">Успішно виконана операція.</response>
        /// <response code="401">Користувач не авторизований.</response>
        [HttpGet]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<ProductResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll()
        {
            // Отримуємо результат бізнес-логіки
            var products = await wishListService.GetAllAsync(CurrentClientAccountId);

            // Формуємо відповідь у форматі ApiResponse та надсилаємо її
            var response = APIResponse<IEnumerable<ProductResponse>>.SuccessResponse(products);
            return Ok(response);
        }

        /// <summary>
        /// Отримати товар зі списку вподобань поточного користувача.
        /// </summary>
        /// <param name="productId">Унікальний ідентифікатор товару (GUID).</param>
        /// <returns>Товар у форматі APIResponse.</returns>
        /// <response code="200">Товар є у списку вподобань.</response>
        /// <response code="401">Користувач не авторизований.</response>
        /// <response code="404">Товару немає у списку вподобань.</response>
        [HttpGet("{productId}")]
        [ProducesResponseType(typeof(APIResponse<ProductResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid productId)
        {
            // Отримуємо результат бізнес-логіки та перевіряємо на null
            var product = await wishListService.GetByIdAsync(CurrentClientAccountId, productId);
            if (product == null) return NotFound(APIResponse.FailureResponse("Product is not in the wishlist"));

            // Формуємо відповідь у форматі ApiResponse та надсилаємо її
            var response = APIResponse<ProductResponse>.SuccessResponse(product);
            return Ok(response);
        }

        /// <summary>
        /// Додати товар до списку вподобань поточного користувача.
        /// </summary>
        /// <param name="productId">Унікальний ідентифікатор товару (GUID).</param>
        /// <returns>Успішну відповідь у форматі APIResponse без data.</returns>
        /// <response code="200">Товар додано (або вже був у списку).</response>
        /// <response code="401">Користувач не авторизований.</response>
        /// <response code="404">Товару з таким ID не існує в базі.</response>
        [HttpPost("{productId}")]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Add(Guid productId)
        {
            // Викликаємо бізнес-логіку для додавання товару
            await wishListService.AddAsync(CurrentClientAccountId, productId);

            // Формуємо відповідь у форматі ApiResponse та надсилаємо її
            logger.LogInformation("Product {productId} added to wishlist by user {userId}", productId, CurrentUserId);
            return Ok(APIResponse.SuccessResponse());
        }

        /// <summary>
        /// Видалити товар зі списку вподобань поточного користувача.
        /// </summary>
        /// <param name="productId">Унікальний ідентифікатор товару (GUID).</param>
        /// <returns>Успішну відповідь у форматі APIResponse без data.</returns>
        /// <response code="200">Товар видалено (або його вже не було у списку).</response>
        /// <response code="401">Користувач не авторизований.</response>
        [HttpDelete("{productId}")]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(Guid productId)
        {
            // Викликаємо бізнес-логіку для видалення товару
            await wishListService.DeleteAsync(CurrentClientAccountId, productId);

            // Формуємо відповідь у форматі ApiResponse та надсилаємо її
            logger.LogInformation("Product {productId} removed from wishlist by user {userId}", productId, CurrentUserId);
            return Ok(APIResponse.SuccessResponse());
        }
    }
}
