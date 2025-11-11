using System.Net.Http.Headers;
using AutoMapper;
using DashboardServer.DTOs;
using Microsoft.AspNetCore.Mvc;
using CourseModel = DashboardServer.Models.Course;

namespace DashboardServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;

        public CourseController(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
        }

        [HttpGet("all/ids")]
        public async Task<IActionResult> GetCourseIds()
        {
            // temporarily hardcoding the API key and base URL in appsettings.Development for testing
            var canvas_api_key = HttpContext.RequestServices.GetRequiredService<IConfiguration>()["Canvas:ApiKey"];
            var base_url = HttpContext.RequestServices.GetRequiredService<IConfiguration>()["Canvas:BaseUrl"];

            var canvasHttpClient = _httpClientFactory.CreateClient();
            canvasHttpClient.BaseAddress = new Uri(base_url);
            canvasHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", canvas_api_key);

            var canvasCourseDTOs = await canvasHttpClient.GetFromJsonAsync<List<CanvasCourseDTO>>("/api/v1/users/self/favorites/courses");
            var courses = _mapper.Map<List<CourseModel>>(canvasCourseDTOs);
            return Ok(courses.Select(c => c.CourseId));
        }

        [HttpGet("all/{courseId}/assignments")]
        public IActionResult GetCourseAssignments(int courseId)
        {
            return Ok($"Retrieved assignments for course with ID: {courseId}");
        }
    }
}