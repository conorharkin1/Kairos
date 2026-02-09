using System.Net.Http.Headers;
using AutoMapper;
using DashboardServer.DTOs.ModelDTOs;
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
            string userId = _userManager.GetUserId(HttpContext.User)!;

            var courses = await _courseService.GetCoursesAsync(userId);
            var courseDtos = _mapper.Map<List<CourseDTO>>(courses);

            return Ok(courseDtos);
        }

        [HttpGet("{courseId}/assignments")]
        public async Task<IActionResult> GetCourseAssignments(int courseId, bool includeCompleted = false)
        {
            string userId = _userManager.GetUserId(HttpContext.User)!;

            var assignments = await _courseService.GetCourseAssignmentsAsync(userId, courseId, includeCompleted);
            var assignmentDtos = _mapper.Map<List<AssignmentDTO>>(assignments);

            return Ok(assignmentDtos);
        }

        [HttpGet("{courseId}/quizzes")]
        public async Task<IActionResult> GetCourseQuizzes(int courseId, bool includeCompleted = false)
        {
            string userId = _userManager.GetUserId(HttpContext.User)!;

            var quizzes = await _courseService.GetCourseQuizzesAsync(userId, courseId, includeCompleted);
            var quizDtos = _mapper.Map<List<QuizDTO>>(quizzes);

            return Ok(quizDtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CourseDTO courseDto)
        {
            string userId = _userManager.GetUserId(HttpContext.User)!;

            var course = _mapper.Map<CourseModel>(courseDto);
            var createdCourse = await _courseService.Create(userId, course);
            return Ok(_mapper.Map<CourseDTO>(createdCourse));
        }
    }
}