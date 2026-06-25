using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MINT.EShop.API.Wrappers;
using MINT.EShop.Business.DTOs.Producers;
using MINT.EShop.Business.Interfaces;

namespace MINT.EShop.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class ProducersController(IProducerService producerService, ILogger<ProducersController> logger) : BaseController
    {
        /// <summary>
        /// Отримати інформацію про всіх виробників товарів, доступних в магазині.
        /// </summary>
        /// <returns>Повертає послідовність всіх виробників у форматі APIResponse.</returns>
        /// <response code="200">Успішно виконана операція.</response>
        [HttpGet]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<ProducerResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            // Отримуємо результат бізнес-логіки
            var producers = await producerService.GetAllAsync();

            // Формуємо відповідь у форматі ApiResponse та надсилаємо її
            var response = APIResponse<IEnumerable<ProducerResponse>>.SuccessResponse(producers);
            return Ok(response);
        }

        /// <summary>
        /// Отримати інформацію про виробника за його ідентифікатором.
        /// </summary>
        /// <param name="id">Унікальний ідентифікатор виробника (GUID).</param>
        /// <returns>Повертає інформацію про виробника у форматі APIResponse.</returns>
        /// <response code="200">Виробник успішно знайдений.</response>
        /// <response code="404">Виробника з таким ID не існує в базі.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(APIResponse<ProducerResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetById(Guid id)
        {
            // Отримуємо результат бізнес-логіки та перевіряємо його на null
            var producer = await producerService.GetByIdAsync(id);
            if (producer == null) return NotFound(APIResponse.FailureResponse("Producer not found"));

            // Формуємо відповідь у форматі ApiResponse
            var response = APIResponse<ProducerResponse>.SuccessResponse(producer);

            // Надсилаємо відповідь
            return Ok(response);
        }

        /// <summary>
        /// Створити нового виробника у магазині. (Manager)
        /// </summary>
        /// <param name="request">CreateProducerRequest (поля: Name, Slug).</param>
        /// <returns>Повертає створеного виробника у форматі APIResponse.</returns>
        /// <response code="201">Виробник успішно створений.</response>
        /// <response code="400">Некоректні дані для створення виробника.</response>
        /// <response code="401">Користувач не авторизований.</response>
        /// <response code="403">Недостатньо прав.</response>
        [HttpPost]
        [Authorize("ManagerPolicy")]
        [ProducesResponseType(typeof(APIResponse<ProducerResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] CreateProducerRequest request)
        {
            // Отримуємо результат бізнес-логіки
            var producer = await producerService.CreateAsync(request);

            // Формуємо відповідь у форматі ApiResponse
            var response = APIResponse<ProducerResponse>.SuccessResponse(producer);

            // Надсилаємо відповідь
            logger.LogInformation("Producer created with ID {ProducerId} by manager {UserId}", producer.Id, CurrentUserId);
            return CreatedAtAction(nameof(GetById), new { id = producer.Id }, response);
        }

        /// <summary>
        /// Оновити інформацію про існуючого виробника за його ідентифікатором. (Manager)
        /// </summary>
        /// <param name="id">Унікальний ідентифікатор виробника (GUID).</param>
        /// <param name="request">UpdateProducerRequest (поля: Name, Slug).</param>
        /// <returns>Повертає оновленого виробника у форматі APIResponse.</returns>
        /// <response code="200">Виробник успішно оновлений.</response>
        /// <response code="404">Виробника з таким ID не існує в базі.</response>
        /// <response code="401">Користувач не авторизований.</response>
        /// <response code="403">Недостатньо прав.</response>
        [HttpPut("{id}")]
        [Authorize("ManagerPolicy")]
        [ProducesResponseType(typeof(APIResponse<ProducerResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProducerRequest request)
        {
            // Отримуємо результат бізнес-логіки та перевіряємо його на null
            var updated = await producerService.UpdateAsync(id, request);
            if (updated == null) return NotFound(APIResponse.FailureResponse("Producer not found"));

            // Формуємо відповідь у форматі ApiResponse та надсилаємо її
            var response = APIResponse<ProducerResponse>.SuccessResponse(updated);
            logger.LogInformation("Producer with ID {ProducerId} updated by manager {UserId}", id, CurrentUserId);
            return Ok(response);
        }

        /// <summary>
        /// Видалити виробника з магазину за його ідентифікатором. (Manager)
        /// </summary>
        /// <param name="id">Унікальний ідентифікатор виробника (GUID).</param>
        /// <returns>Повертає ID видаленого виробника.</returns>
        /// <response code="200">Виробник успішно видалений.</response>
        /// <response code="404">Виробника з таким ID не існує в базі.</response>
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
            var isDeleted = await producerService.DeleteAsync(id);

            // Перевіряємо результат та формуємо відповідь у форматі ApiResponse
            if (!isDeleted) return NotFound(APIResponse.FailureResponse($"Producer with ID {id} not found"));
            logger.LogInformation("Producer with ID {ProducerId} deleted by manager {UserId}", id, CurrentUserId);
            return Ok(APIResponse<Guid>.SuccessResponse(id));
        }
    }
}
