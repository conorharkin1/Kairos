using AutoMapper;
using DashboardServer.DTOs.ModelDTOs;
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
        private readonly IMapper _mapper;
        private readonly IQuizService _quizService;


        public QuizController(UserManager<ApplicationUser> userManager, IMapper mapper, IQuizService quizService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _quizService = quizService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuiz([FromBody] QuizDTO quizDto)
        {
            string userId = _userManager.GetUserId(HttpContext.User)!;

            var quiz = _mapper.Map<Quiz>(quizDto);
            var createdQuiz = await _quizService.Create(userId, quiz);
            return Ok(_mapper.Map<QuizDTO>(createdQuiz));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuiz(int id, [FromBody] QuizDTO quizDto)
        {
            string userId = _userManager.GetUserId(HttpContext.User)!;

            var quiz = _mapper.Map<Quiz>(quizDto);
            var updatedQuiz = await _quizService.Update(userId, id, quiz);
            return Ok(_mapper.Map<QuizDTO>(updatedQuiz));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            string userId = _userManager.GetUserId(HttpContext.User)!;

            await _quizService.Delete(userId, id);
            return NoContent();
        }
    }
}
