using DashboardServer.DTOs.ModelDTOs.Requests;
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
    public class QuizController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IQuizService _quizService;

        public QuizController(UserManager<ApplicationUser> userManager, IQuizService quizService)
        {
            _userManager = userManager;
            _quizService = quizService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var quizzes = await _quizService.GetAllAsync();
            return Ok(quizzes);
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuiz([FromBody] QuizRequestDTO dto)
        {
            string userId = _userManager.GetUserId(HttpContext.User)!;
            var result = await _quizService.CreateAsync(userId, dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuiz(int id, [FromBody] QuizRequestDTO dto)
        {
            string userId = _userManager.GetUserId(HttpContext.User)!;
            var result = await _quizService.UpdateAsync(userId, id, dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            string userId = _userManager.GetUserId(HttpContext.User)!;
            await _quizService.DeleteAsync(userId, id);
            return NoContent();
        }
    }
}
