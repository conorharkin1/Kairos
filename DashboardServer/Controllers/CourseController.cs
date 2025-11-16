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

        public CourseController(IMapper mapper, UserManager<ApplicationUser> userManager, ICanvasApiService canvasApiService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _canvasApiService = canvasApiService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetCourses()
        {
            string? userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
            {
                return Unauthorized("User not found");
            }
            var canvasCourseDtos = await _canvasApiService.GetFavoriteCoursesAsync(userId);
            var courses = _mapper.Map<List<CourseModel>>(canvasCourseDtos);

            return Ok(_mapper.Map<List<CourseDTO>>(courses));
        }

        [HttpGet("{courseId}/assignments")]
        public async Task<IActionResult> GetCourseAssignments(int courseId)
        {
            string? userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
            {
                return Unauthorized("User not found");
            }
            var canvasAssignmentDtos = await _canvasApiService.GetCourseAssignmentsAsync(userId, courseId);
            var assignments = _mapper.Map<List<AssignmentModel>>(canvasAssignmentDtos);
            return Ok(_mapper.Map<List<AssignmentDTO>>(assignments));
        }
    }
}