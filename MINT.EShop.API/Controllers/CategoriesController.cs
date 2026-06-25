using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MINT.EShop.API.Wrappers;
using MINT.EShop.Business.DTOs.Category;
using MINT.EShop.Business.Interfaces;

namespace MINT.EShop.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class CategoriesController(ICategoryService categoryService, ILogger<CategoriesController> logger) : BaseController
    {
        /// <summary>
        /// Отримати інформацію про всі категорії товарів, доступні в магазині.
        /// </summary>
        /// <returns>Повертає послідовність всіх категорій у форматі APIResponse.</returns>
        /// <response code="200">Успішно виконана операція.</response>
        [HttpGet]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<CategoryResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            // Отримуємо результат бізнес-логіки
            var categories = await categoryService.GetAllAsync();

            // Формуємо відповідь у форматі ApiResponse та надсилаємо її
            var response = APIResponse<IEnumerable<CategoryResponse>>.SuccessResponse(categories);
            return Ok(response);
        }

        /// <summary>
        /// Отримати інформацію про категорію за його ідентифікатором.
        /// </summary>
        /// <param name="id">Унікальний ідентифікатор категорії (GUID).</param>
        /// <returns>Повертає інформацію про категорію у форматі APIResponse.</returns>
        /// <response code="200">Категорія успішно знайдена.</response>
        /// <response code="404">Категорії з таким ID не існує в базі.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(APIResponse<CategoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetById(Guid id)
        {
            // Отримуємо результат бізнес-логіки та перевіряємо його на null
            var category = await categoryService.GetByIdAsync(id);
            if (category == null) return NotFound(APIResponse.FailureResponse("Category not found"));

            // Формуємо відповідь у форматі ApiResponse
            var response = APIResponse<CategoryResponse>.SuccessResponse(category);

            // Надсилаємо відповідь
            return Ok(response);
        }

        /// <summary>
        /// Створити нову категорію у магазині. (Manager)
        /// </summary>
        /// <param name="request">CreateCategoryRequest (поля: Name, Slug).</param>
        /// <returns>Повертає створену категорію у форматі APIResponse.</returns>
        /// <response code="201">Категорія успішно створена.</response>
        /// <response code="400">Некоректні дані для створення категорії.</response>
        /// <response code="401">Користувач не авторизований.</response>
        /// <response code="403">Недостатньо прав.</response>
        [HttpPost]
        [Authorize("ManagerPolicy")]    
        [ProducesResponseType(typeof(APIResponse<CategoryResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
        {
            // Отримуємо результат бізнес-логіки
            var category = await categoryService.CreateAsync(request);

            // Формуємо відповідь у форматі ApiResponse
            var response = APIResponse<CategoryResponse>.SuccessResponse(category);

            // Надсилаємо відповідь
            logger.LogInformation("Category created with ID {CategoryId} by manager {UserId}", category.Id, CurrentUserId);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, response);
        }

        /// <summary>
        /// Оновити інформацію про існуючу категорію за її ідентифікатором. (Manager)
        /// </summary>
        /// <param name="id">Унікальний ідентифікатор категорії (GUID).</param>
        /// <param name="request">UpdateCategoryRequest (поля: Name, Slug).</param>
        /// <returns>Повертає оновлену категорію у форматі APIResponse.</returns>
        /// <response code="200">Категорія успішно оновлена.</response>
        /// <response code="404">Категорії з таким ID не існує в базі.</response>
        /// <response code="401">Користувач не авторизований.</response>
        /// <response code="403">Недостатньо прав.</response>
        [HttpPut("{id}")]
        [Authorize("ManagerPolicy")]
        [ProducesResponseType(typeof(APIResponse<CategoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryRequest request)
        {
            // Отримуємо результат бізнес-логіки та перевіряємо його на null
            var updated = await categoryService.UpdateAsync(id, request);
            if (updated == null) return NotFound(APIResponse.FailureResponse("Category not found"));

            // Формуємо відповідь у форматі ApiResponse та надсилаємо її
            var response = APIResponse<CategoryResponse>.SuccessResponse(updated);
            logger.LogInformation("Category with ID {CategoryId} updated by manager {UserId}", id, CurrentUserId);
            return Ok(response);
        }

        /// <summary>
        /// Видалити категорію з магазину за її ідентифікатором. (Manager)
        /// </summary>
        /// <param name="id">Унікальний ідентифікатор категорії (GUID).</param>
        /// <returns>Повертає ID видаленої категорії.</returns>
        /// <response code="200">Категорія успішно видалена.</response>
        /// <response code="404">Категорії з таким ID не існує в базі.</response>
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
            var isDeleted = await categoryService.DeleteAsync(id);

            // Перевіряємо результат та формуємо відповідь у форматі ApiResponse
            if (!isDeleted) return NotFound(APIResponse.FailureResponse($"Category with ID {id} not found"));
            logger.LogInformation("Category with ID {CategoryId} deleted by manager {UserId}", id, CurrentUserId);
            return Ok(APIResponse<Guid>.SuccessResponse(id));
        }
    }
}
