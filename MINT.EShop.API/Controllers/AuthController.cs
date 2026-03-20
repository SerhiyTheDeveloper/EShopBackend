using MINT.EShop.Business.DTOs.Identity;
using Microsoft.AspNetCore.Mvc;
using MINT.EShop.Business.Interfaces;
using MINT.EShop.API.Wrappers;

namespace MINT.EShop.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        /// <summary>
        /// Ввід в систему.
        /// </summary>
        /// <param name="request">LoginRequest (поля: Email, Password)</param>
        /// <returns>Повертає об'єкт LoginResponse</returns>
        /// <response code="200">Користувача знайдено.</response>
        /// <response code="400">Некоректно введені дані.</response>
        /// <response code="401">Користувача не знайдено.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(APIResponse<LoginResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Отримуємо результат бізнес-логіки
            var result =  await authService.Login(request);

            // Перевіряємо на null
            if (result == null)
            {
                return Unauthorized(APIResponse.FailureResponse("Invalid email or password"));
            }

            // Формуємо відповідь у форматі ApiResponse та надсилаємо її
            var response = APIResponse<LoginResponse>.SuccessResponse(result);
            return Ok(response);
        }
    }
}
