using System.Net.Http.Headers;
using AutoMapper;
using DashboardServer.DTOs;
using DashboardServer.DTOs.ResponseDTOs;
using Microsoft.AspNetCore.Mvc;
using CourseModel = DashboardServer.Models.Course;
using AssignmentModel = DashboardServer.Models.Assignment;
using Microsoft.AspNetCore.Identity;
using DashboardServer.Models;
using DashboardServer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace DashboardServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CourseController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICanvasApiService _canvasApiService;
        private readonly ICourseService _courseService;

        public CourseController(IMapper mapper, UserManager<ApplicationUser> userManager, ICanvasApiService canvasApiService, ICourseService courseService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _canvasApiService = canvasApiService;
            _courseService = courseService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllCourses()
        {
            string? userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
            {
                return Unauthorized("User not found");
            }

            var courses = await _courseService.GetCoursesAsync(userId);
            var courseDtos = _mapper.Map<List<CourseDTO>>(courses);

            return Ok(courseDtos);
        }

        [HttpGet("{courseId}/assignments")]
        public async Task<IActionResult> GetCourseAssignments(int courseId)
        {
            string? userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
            {
                return Unauthorized("User not found");
            }

            var assignments = await _courseService.GetCourseAssignmentsAsync(userId, courseId);
            var assignmentDtos = _mapper.Map<List<AssignmentDTO>>(assignments);

            return Ok(assignmentDtos);
        }

        [HttpGet("{courseId}/quizzes")]
        public async Task<IActionResult> GetCourseQuizzes(int courseId)
        {
            string? userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
            {
                return Unauthorized("User not found");
            }

            var quizzes = await _courseService.GetCourseQuizzesAsync(userId, courseId);
            var quizDtos = _mapper.Map<List<QuizDTO>>(quizzes);

            return Ok(quizDtos);
        }
    }
}