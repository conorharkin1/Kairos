using DashboardServer.Models;
using DashboardServer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DashboardServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        public UserController(UserManager<ApplicationUser> userManager, IUserService userService)
        {
            _userManager = userManager;
            _userService = userService;
        }

        [HttpPost("key")]
        public async Task<IActionResult> SetApiKey([FromBody] string apiKey)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                user = user ?? throw new Exception("User not found");
                var userId = user.Id;
                // I'm creating a new guid here based on the userId to use as a key for encryption - not suitable for production but for demo it'll be fine
                Guid encryptionKey = Guid.Parse(userId);

                await _userService.SetApiKey(apiKey, encryptionKey, userId);
                
                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"An error occurred while setting the API key: {ex}");
            }
        }
    }
}