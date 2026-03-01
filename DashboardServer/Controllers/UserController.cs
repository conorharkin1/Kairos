using AutoMapper;
using DashboardServer.DTOs.AuthDTOs;
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

            await _userService.SetApiKey(apiKey, userId);
            
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