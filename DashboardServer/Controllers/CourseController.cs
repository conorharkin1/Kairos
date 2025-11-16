using System.Net.Http.Headers;
using AutoMapper;
using DashboardServer.DTOs;
using DashboardServer.DTOs.ResponseDTOs;
using Microsoft.AspNetCore.Mvc;
using CourseModel = DashboardServer.Models.Course;
using AssignmentModel = DashboardServer.Models.Assignment;

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

        [HttpGet("ids")]
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
            return Ok(_mapper.Map<List<CourseDTO>>(courses));
        }

        [HttpGet("{courseId}/assignments")]
        public async Task<IActionResult> GetCourseAssignments(int courseId)
        {
            // temporarily hardcoding the API key and base URL in appsettings.Development for testing
            var canvas_api_key = HttpContext.RequestServices.GetRequiredService<IConfiguration>()["Canvas:ApiKey"];
            var base_url = HttpContext.RequestServices.GetRequiredService<IConfiguration>()["Canvas:BaseUrl"];

            var canvasHttpClient = _httpClientFactory.CreateClient();
            canvasHttpClient.BaseAddress = new Uri(base_url);
            canvasHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", canvas_api_key);

            var canvasAssignmentDTOs = await canvasHttpClient.GetFromJsonAsync<List<CanvasAssignmentDTO>>($"/api/v1/courses/{courseId}/assignments");
            var assignments = _mapper.Map<List<AssignmentModel>>(canvasAssignmentDTOs);
            return Ok(_mapper.Map<List<AssignmentDTO>>(assignments));
        }
    }
}