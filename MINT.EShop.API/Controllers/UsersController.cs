using Microsoft.AspNetCore.Mvc;
using MINT.EShop.API.Wrappers;
using MINT.EShop.Business.DTOs.Identity;
using MINT.EShop.Business.Interfaces;
using MINT.EShop.Core.Entities.UserData;

namespace MINT.EShop.API.Controllers
{

    namespace MINT.EShop.API.Controllers
    {
        [ApiController]
        [Route("api/v1/[controller]")]
        [Produces("application/json")]
        public class UsersController(IUserService userService) : ControllerBase
        {
            /// <summary>
            /// Отримати інформацію про всіх користувачів.
            /// </summary>
            /// <returns>Повертає послідовність всіх користувачів у форматі APIResponse.</returns>
            /// <response code="200">Успішно виконана операція.</response>
            [HttpGet]
            [ProducesResponseType(typeof(APIResponse<IEnumerable<UserResponse>>), StatusCodes.Status200OK)]
            public async Task<IActionResult> GetAll()
            {
                // Отримуємо результат бізнес-логіки
                var users = await userService.GetAllAsync();

                // Формуємо відповідь у форматі ApiResponse та надсилаємо її
                var response = APIResponse<IEnumerable<UserResponse>>.SuccessResponse(users);
                return Ok(response);
            }

            /// <summary>
            /// Отримати інформацію про користувача за його унікальним ідентифікатором (ID).
            /// </summary>
            /// <param name="id">Унікальний ідентифікатор користувача (GUID).</param>
            /// <returns>Повертає інформацію про користувача у форматі APIResponse.</returns>
            /// <response code="200">Користувача успішно знайдено.</response>
            /// <response code="404">Користувача з таким ID не існує в базі.</response>
            [HttpGet("{id}")]
            [ProducesResponseType(typeof(APIResponse<UserResponse>), StatusCodes.Status200OK)]
            [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
            public async Task<IActionResult> GetById(Guid id)
            {
                // Отримуємо результат бізнес-логіки та перевіряємо його на null
                var user = await userService.GetByIdAsync(id);
                if (user == null) return NotFound(APIResponse.FailureResponse("User not found"));

                // Формуємо відповідь у форматі ApiResponse та надсилаємо її
                var response = APIResponse<UserResponse>.SuccessResponse(user);
                return Ok(response);
            }

            /// <summary>
            /// Створити (зареєструвати) нового користувача з наданими даними.
            /// </summary>
            /// <param name="request">RegisterRequest (поля: Email, Password, FirstName, LastName (необов'язково)).</param>
            /// <returns>Повертає створеного користувача у форматі APIResponse.</returns>
            /// <response code="201">Користувача успішно створено.</response>
            /// <response code="400">Некоректні дані для створення користувача.</response>
            [HttpPost]
            [ProducesResponseType(typeof(APIResponse<UserResponse>), StatusCodes.Status201Created)]
            [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
            public async Task<IActionResult> Create([FromBody]RegisterRequest request)
            {
                // Отримуємо результат бізнес-логіки
                var user = await userService.CreateAsync(request);

                // Формуємо відповідь у форматі ApiResponse
                var response = APIResponse<UserResponse>.SuccessResponse(user);

                // Надсилаємо відповідь
                return CreatedAtAction(nameof(GetById), new { id = user.Id }, response);
            }

            /// <summary>
            /// Оновити інформацію про користувача за його унікальним ідентифікатором (ID) з наданими даними.
            /// </summary>
            /// <param name="id">Унікальний ідентифікатор користувача (GUID).</param>
            /// <param name="request">UpdateUserDataRequest (поля: FirstName, LastName (необов'язково)).</param>
            /// <returns>Повертає оновлену інформацію про користувача у форматі APIResponse.</returns>
            /// <response code="200">Користувача успішно оновлено.</response>
            /// <response code="404">Користувача з таким ID не існує в базі.</response>
            /// <response code="400">Некоректні дані для оновлення користувача.</response>
            [HttpPut("{id}")]
            [ProducesResponseType(typeof(APIResponse<UserResponse>), StatusCodes.Status200OK)]
            [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
            [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
            public async Task<IActionResult> Update(Guid id, UpdateUserDataRequest request)
            {
                // Отримуємо результат бізнес-логіки та перевіряємо його на null
                var updated = await userService.UpdateDataAsync(id, request);
                if (updated == null) return NotFound(APIResponse.FailureResponse("User not found"));

                // Формуємо відповідь у форматі ApiResponse та надсилаємо її
                var response = APIResponse<UserResponse>.SuccessResponse(updated);
                return Ok(response);
            }

            /// <summary>
            /// Видалити користувача за його унікальним ідентифікатором (ID).
            /// </summary>
            /// <param name="id">Унікальний ідентифікатор користувача (GUID).</param>
            /// <returns>Повертає ID видаленого користувача.</returns>
            /// <response code="200">Користувача успішно видалено.</response>
            /// <response code="404">Користувача з таким ID не існує в базі.</response>
            [HttpDelete("{id}")]
            [ProducesResponseType(typeof(APIResponse<Guid>), StatusCodes.Status200OK)]
            [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
            public async Task<IActionResult> Delete(Guid id)
            {
                // Отримуємо результат бізнес-логіки
                bool isDeleted = await userService.DeleteAsync(id);

                // Перевіряємо результат та формуємо відповідь у форматі ApiResponse
                if (!isDeleted) return NotFound(APIResponse.FailureResponse("User not found"));
                return Ok(APIResponse<Guid>.SuccessResponse(id));
            }
        }
    }
}
