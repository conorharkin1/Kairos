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
    public class CourseController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICourseService _courseService;

        public CourseController(UserManager<ApplicationUser> userManager, ICourseService courseService)
        {
            _userManager = userManager;
            _courseService = courseService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _courseService.GetAllAsync();
            return Ok(courses);
        }

        [HttpGet("{courseId}/assignments")]
        public async Task<IActionResult> GetCourseAssignments(int courseId, bool includeCompleted = false)
        {
            var assignments = await _courseService.GetCourseAssignmentsAsync(courseId, includeCompleted);
            return Ok(assignments);
        }

        [HttpGet("{courseId}/quizzes")]
        public async Task<IActionResult> GetCourseQuizzes(int courseId, bool includeCompleted = false)
        {
            var quizzes = await _courseService.GetCourseQuizzesAsync(courseId, includeCompleted);
            return Ok(quizzes);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CourseRequestDTO courseDto)
        {
            string userId = _userManager.GetUserId(HttpContext.User)!;
            var result = await _courseService.CreateAsync(userId, courseDto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseRequestDTO dto)
        {
            string userId = _userManager.GetUserId(HttpContext.User)!;
            var result = await _courseService.UpdateAsync(userId, id, dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            string userId = _userManager.GetUserId(HttpContext.User)!;
            await _courseService.DeleteAsync(userId, id);
            return NoContent();
        }
    }
}