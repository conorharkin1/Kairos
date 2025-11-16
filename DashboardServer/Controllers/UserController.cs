using AutoMapper;
using DashboardServer.DTOs;
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
        private readonly IMapper _mapper;
        public UserController(UserManager<ApplicationUser> userManager, IUserService userService, IMapper mapper)
        {
            _userManager = userManager;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("self")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(User);
            
            if (user != null)
            {
                return Ok(_mapper.Map<UserDTO>(user)); 
            }

            return StatusCode(404, "User not found");
        }

        [HttpPost("key")]
        public async Task<IActionResult> SetApiKey([FromBody] string apiKey)
        {
            try
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                userId = userId ?? throw new Exception("User not found");
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