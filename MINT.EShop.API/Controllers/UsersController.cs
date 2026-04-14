using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MINT.EShop.API.Wrappers;
using MINT.EShop.Business.DTOs.Identity;
using MINT.EShop.Business.Interfaces;

namespace MINT.EShop.API.Controllers
{

    namespace MINT.EShop.API.Controllers
    {
        [ApiController]
        [Route("api/v1/[controller]")]
        [Produces("application/json")]
        public class UsersController(IUserService userService, ILogger<UsersController> logger) : BaseController
        {
            /// <summary>
            /// Отримати інформацію про всіх користувачів. (Admin)
            /// </summary>
            /// <returns>Список об'єктів UserResponse у форматі APIResponse.</returns>
            /// <response code="200">Успішно виконана операція.</response>
            /// <response code="401">Користувач не авторизований.</response>
            /// <response code="403">Недостатньо прав</response>
            [HttpGet]
            [Authorize("AdminPolicy")]
            [ProducesResponseType(typeof(APIResponse<IEnumerable<UserResponse>>), StatusCodes.Status200OK)]
            [ProducesResponseType(typeof(APIResponse), StatusCodes.Status401Unauthorized)]
            [ProducesResponseType(typeof(APIResponse), StatusCodes.Status403Forbidden)]
            public async Task<IActionResult> GetAll()
            {
                // Отримуємо результат бізнес-логіки
                var users = await userService.GetAllAsync();

                // Формуємо відповідь у форматі ApiResponse та надсилаємо її
                var response = APIResponse<IEnumerable<UserResponse>>.SuccessResponse(users);
                logger.LogInformation("List of users requested by Admin");
                return Ok(response);
            }

            /// <summary>
            /// Зареєструвати нового користувача.
            /// </summary>
            /// <param name="request">RegisterRequest (поля: Email, Password, FirstName, LastName (необов'язково)).</param>
            /// <returns>Об'єкт UserResponse у форматі APIResponse.</returns>
            /// <response code="200">Користувача зареєстровано.</response>
            /// <response code="400">Некоректні дані для реєстрації користувача.</response>
            [HttpPost("register")]
            [ProducesResponseType(typeof(APIResponse<UserResponse>), StatusCodes.Status201Created)]
            [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
            public async Task<IActionResult> Register([FromBody] RegisterRequest request)
            {
                // Отримуємо результат бізнес-логіки
                var result = await userService.RegisterAsync(request);

                // Формуємо відповідь у форматі ApiResponse
                var response = APIResponse<RegisterResponse>.SuccessResponse(result,
                    "User registered successfully. A verification code has been sent to specified email");

                // Надсилаємо відповідь
                return Ok(response);
            }
            
            /// <summary>
            /// Верифікувати користувача.
            /// </summary>
            /// <param name="request">VerifyRequest (поля: Email, VerificationCode).</param>
            /// <returns>Об'єкт UserResponse у форматі APIResponse.</returns>
            /// <response code="201">Користувача верифіковано та створено.</response>
            /// <response code="400">Некоректні дані для верифікації користувача.</response>
            [HttpPost("verify")]
            [ProducesResponseType(typeof(APIResponse<UserResponse>), StatusCodes.Status200OK)]
            [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
            public async Task<IActionResult> Verify([FromBody] VerifyRequest request)
            {
                // Отримуємо результат бізнес-логіки
                var result = await userService.VerifyAsync(request);
                
                // Перевіряємо чи зійшлися верифікаційні коди
                if (result == null) return BadRequest(APIResponse.FailureResponse("Uncorrect verification code"));
                
                // Формуємо відповідь у форматі ApiResponse
                var response = APIResponse<UserResponse>.SuccessResponse(result, "User verified and created successfully.");
                
                // Надсилаємо відповідь
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, response);
            }

            /// <summary>
            /// Отримати інформацію про користувача за його унікальним ідентифікатором (ID). (Admin)
            /// </summary>
            /// <param name="id">Унікальний ідентифікатор користувача (GUID).</param>
            /// <returns>Об'єкт UserResponse у форматі APIResponse.</returns>
            /// <response code="200">Користувача успішно знайдено.</response>
            /// <response code="404">Користувача з таким ID не існує в базі.</response>
            /// <response code="401">Користувач не авторизований.</response>
            /// <response code="403">Недостатньо прав</response>
            [HttpGet("{id}")]
            [Authorize("AdminPolicy")]
            [ProducesResponseType(typeof(APIResponse<UserResponse>), StatusCodes.Status200OK)]
            [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
            [ProducesResponseType(typeof(APIResponse), StatusCodes.Status401Unauthorized)]
            [ProducesResponseType(typeof(APIResponse), StatusCodes.Status403Forbidden)]
            public async Task<IActionResult> GetById(Guid id)
            {
                // Отримуємо результат бізнес-логіки та перевіряємо на null
                var user = await userService.GetByIdAsync(id);
                if (user == null) return NotFound(APIResponse.FailureResponse("User not found"));

                // Формуємо відповідь у форматі ApiResponse та надсилаємо її
                var response = APIResponse<UserResponse>.SuccessResponse(user);
                logger.LogInformation("User with id {id} requested by Admin", id);
                return Ok(response);
            }

            /// <summary>
            /// Отримати інформацію про поточного користувача
            /// </summary>
            /// <returns>Об'єкт UserResponse у форматі APIResponse.</returns>
            /// <response code="200">Користувача успішно знайдено.</response>
            /// <response code="404">Користувача з таким ID не існує в базі.</response>
            /// <response code="401">Користувач не авторизований.</response>
            [HttpGet("me")]
            [Authorize]
            [ProducesResponseType(typeof(APIResponse<UserResponse>), StatusCodes.Status200OK)]
            [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
            [ProducesResponseType(typeof(APIResponse), StatusCodes.Status401Unauthorized)]
            public async Task<IActionResult> GetCurrentData()
            {
                // Отримуємо інформацію про поточного користувача за його ID та перевіряємо на null
                var user = await userService.GetByIdAsync(CurrentUserId);
                if (user == null)
                {
                    logger.LogWarning("User with id {id} not found. User requested himself.", CurrentUserId);
                    return NotFound(APIResponse.FailureResponse("User does not exist in the database."));
                }

                // Формуємо відповідь у форматі ApiResponse та надсилаємо її
                var response = APIResponse<UserResponse>.SuccessResponse(user);
                return Ok(response);
            }

            /// <summary>
            /// Оновити інформацію про користувача за його унікальним ідентифікатором (ID). (Admin)
            /// </summary>
            /// <param name="id">Унікальний ідентифікатор користувача (GUID).</param>
            /// <param name="request">UpdateUserDataRequest (поля: FirstName, LastName (необов'язково)).</param>
            /// <returns>Об'єкт UserResponse у форматі APIResponse.</returns>
            /// <response code="200">Дані користувача успішно оновлено.</response>
            /// <response code="404">Користавача з таким ID не існує в базі.</response>
            /// <response code="400">Некоректно передані дані.</response>
            /// <response code="401">Користувач не авторизований.</response>
            /// <response code="403">Недостатньо прав.</response>
            [HttpPut("{id}")]
            [Authorize("AdminPolicy")]
            [ProducesResponseType(typeof(APIResponse<UserResponse>), StatusCodes.Status200OK)]
            [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
            [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
            [ProducesResponseType(typeof(APIResponse), StatusCodes.Status401Unauthorized)]
            [ProducesResponseType(typeof(APIResponse), StatusCodes.Status403Forbidden)]
            public async Task<IActionResult> UpdateData(Guid id, UpdateUserDataRequest request)
            {
                // Отримуємо результат бізнес-логіки та перевіряємо його на null
                var updated = await userService.UpdateDataAsync(id, request);
                if (updated == null) return NotFound(APIResponse.FailureResponse("User not found"));

                // Формуємо відповідь у форматі ApiResponse та надсилаємо її
                var response = APIResponse<UserResponse>.SuccessResponse(updated);
                logger.LogInformation("User with id {id} updated by Admin", id);
                return Ok(response);
            }

            /// <summary>
            /// Оновити інформацію про поточного користувача.
            /// </summary>
            /// <param name="request">UpdateUserDataRequest (поля: FirstName, LastName (необов'язково)).</param>
            /// <returns>Об'єкт UserResponse у форматі APIResponse.</returns>
            /// <response code="200">Дані користувача успішно оновлено.</response>
            /// <response code="404">Користавача з таким ID не існує в базі.</response>
            /// <response code="400">Некоректно передані дані.</response>
            /// <response code="401">Користувач не авторизований.</response>
            [HttpPut("me")]
            [Authorize]
            [ProducesResponseType(typeof(APIResponse<UserResponse>), StatusCodes.Status200OK)]
            [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
            [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
            [ProducesResponseType(typeof(APIResponse), StatusCodes.Status401Unauthorized)]
            public async Task<IActionResult> UpdateCurrentData(UpdateUserDataRequest request)
            {
                // Отримуємо результат бізнес-логіки та перевіряємо його на null
                var updated = await userService.UpdateDataAsync(CurrentUserId, request);
                if (updated == null)
                {
                    logger.LogWarning("User with id {id} not found. User requested update of himself.", CurrentUserId);
                    return NotFound(APIResponse.FailureResponse("User does not exist in the database."));
                }

                // Формуємо відповідь у форматі ApiResponse та надсилаємо її
                var response = APIResponse<UserResponse>.SuccessResponse(updated);
                return Ok(response);
            }

            /// <summary>
            /// Видалити користувача за його унікальним ідентифікатором (ID). (Admin)
            /// </summary>
            /// <param name="id">Унікальний ідентифікатор користувача (GUID).</param>
            /// <returns>ID видаленого користувача у форматі APIResponse.</returns>
            /// <response code="200">Користувача успішно видалено.</response>
            /// <response code="404">Користувача з таким ID не існує в базі.</response>
            /// <response code="401">Користувач не авторизований.</response>
            /// <response code="403">Недостатньо прав</response>
            [HttpDelete("{id}")]
            [Authorize("AdminPolicy")]
            [ProducesResponseType(typeof(APIResponse<Guid>), StatusCodes.Status200OK)]
            [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
            [ProducesResponseType(typeof(APIResponse), StatusCodes.Status401Unauthorized)]
            [ProducesResponseType(typeof(APIResponse), StatusCodes.Status403Forbidden)]
            public async Task<IActionResult> Delete(Guid id)
            {
                // Отримуємо результат бізнес-логіки
                bool isDeleted = await userService.DeleteAsync(id);

                // Перевіряємо результат та надсилаємо відповідь у форматі ApiResponse
                if (!isDeleted) return NotFound(APIResponse.FailureResponse("User not found"));
                logger.LogInformation("User with id {id} deleted by Admin", id);
                return Ok(APIResponse<Guid>.SuccessResponse(id));
            }
        }
    }
}