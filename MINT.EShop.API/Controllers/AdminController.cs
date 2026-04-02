using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MINT.EShop.API.Wrappers;
using MINT.EShop.Business.Interfaces;

namespace MINT.EShop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize("AdminPolicy")]
    [Produces("application/json")]
    public class AdminController(IUserService userService) : ControllerBase
    {
        /// <summary>
        /// Метод підвищення клієнта до менеджера.
        /// </summary>
        /// <param name="userId">Унікальний ідентифікатор користувача.</param>
        /// <returns>Успішну відповідь у форматі APIResponse без data</returns>
        /// <response code="200">Операцію успішно виконано</response>
        /// <response code="404">Користувача не знайдено</response>
        [HttpPost("users/{userId}/promote")]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PromoteToManager(Guid userId)
        {
            var result = await userService.PromoteToManagerAsync(userId);

            if (!result) return NotFound(APIResponse.FailureResponse("User not found"));

            return Ok(APIResponse.SuccessResponse());
        }

        /// <summary>
        /// Метод пониження менеджера до клієнта.
        /// </summary>
        /// <param name="userId">Унікальний ідентифікатор користувача.</param>
        /// <returns>Успішну відповідь у форматі APIResponse без data</returns>
        /// <response code="200">Операцію успішно виконано</response>
        /// <response code="404">Користувача не знайдено</response>
        [HttpPost("users/{userId}/demote")]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DemoteToClient(Guid userId)
        {
            var result = await userService.DemoteToClientAsync(userId);

            if (!result) return NotFound(APIResponse.FailureResponse("User not found"));

            return Ok(APIResponse.SuccessResponse());
        }
    }
}