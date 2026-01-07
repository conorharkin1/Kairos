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
        private readonly ICanvasApiService _canvasApiService;
        public UserController(UserManager<ApplicationUser> userManager, IUserService userService, IMapper mapper, ICanvasApiService canvasApiService)
        {
            _userManager = userManager;
            _userService = userService;
            _mapper = mapper;
            _canvasApiService = canvasApiService;
        }

        [HttpGet("self")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(User);

            return Ok(_mapper.Map<UserDTO>(user));
        }

        [HttpPost("key")]
        public async Task<IActionResult> SetApiKey([FromBody] string apiKey)
        {
            var userId = _userManager.GetUserId(HttpContext.User)!;

            // I'm creating a new guid here based on the userId to use as a key for encryption - not suitable for production but for demo it'll be fine
            Guid encryptionKey = Guid.Parse(userId);

            await _userService.SetApiKey(apiKey, encryptionKey, userId);
            
            return Ok();
        }

        [HttpPost("refresh/canvas")]
        public async Task<IActionResult> RefreshCanvasData()
        {
            var userId = _userManager.GetUserId(HttpContext.User)!;

            await _canvasApiService.UpdateDataFromCanvasAsync(userId);
            return Ok();
        }
    }
}