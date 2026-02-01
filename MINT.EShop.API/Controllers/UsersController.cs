using Microsoft.AspNetCore.Mvc;

namespace MINT.EShop.API.Controllers
{
    using global::MINT.EShop.Business.Interfaces;
    using global::MINT.EShop.Core.Entities;
    using Microsoft.AspNetCore.Mvc;

    namespace MINT.EShop.API.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class UsersController : ControllerBase
        {
            private readonly IUserService _userService;

            public UsersController(IUserService userService)
            {
                _userService = userService;
            }

            [HttpGet]
            public async Task<IActionResult> GetAll()
            {
                var users = await _userService.GetAllAsync();
                return Ok(users);
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetById(Guid id)
            {
                var user = await _userService.GetByIdAsync(id);
                if (user == null) return NotFound();
                return Ok(user);
            }

            [HttpPost]
            public async Task<IActionResult> Create(User user)
            {
                var created = await _userService.CreateAsync(user);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> Update(Guid id, User user)
            {
                user.Id = id;
                var updated = await _userService.UpdateAsync(user);
                if (updated == null) return NotFound();
                return Ok(updated);
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(Guid id)
            {
                var deleted = await _userService.DeleteAsync(id);
                if (!deleted) return NotFound();
                return NoContent();
            }
        }
    }



}
