using System.Security.Cryptography;
using System.Threading.Tasks;
using DashboardServer.Models;
using DashboardServer.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DashboardServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProfileService _profileService;
        public ProfileController(UserManager<ApplicationUser> userManager, IProfileService profileService)
        {
            _userManager = userManager;
            _profileService = profileService;
        }

        [HttpPost("key")]
        public async Task<IActionResult> SetApiKey([FromBody] string key)
        {
            try
            {
                // TODO add some authentication for users, currently this always returns null as I have no auth set up.
                var user = await _userManager.GetUserAsync(User);
                user = user ?? throw new Exception("User not found");
                var userId = user.Id;

                await _profileService.SetApiKey(userId, key);
                
                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"An error occurred while setting the API key: {ex}");
            }
        }
    }
}