using DashboardServer.DTOs;
using DashboardServer.Models;
using DashboardServer.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DashboardServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.RegisterAsync(dto);

            if (user == null)
                return BadRequest("Registration failed. Please try again.");

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            try
            {
                var user = await _userService.LoginAsync(dto);
                return Ok(user);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Invalid email or password.");
            }
        }

    }
}