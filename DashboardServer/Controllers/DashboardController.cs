using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;

namespace DashboardServer.Controllers
{
    [ApiController]
    [Route("api/")]
    public class DashboardController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public DashboardController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("courses")]
        public IActionResult GetCourses()
        {
            return Ok(new[] { "Course 1", "Course 2" });
        }

        [HttpGet("courses/ids")]
        public async Task<IActionResult> GetCourseIds()
        {
            // temporarily hardcoding the API key and base URL in appsettings.Development for testing
            var canvas_api_key = HttpContext.RequestServices.GetRequiredService<IConfiguration>()["Canvas:ApiKey"];
            var base_url = HttpContext.RequestServices.GetRequiredService<IConfiguration>()["Canvas:BaseUrl"];

            var canvasHttpClient = _httpClientFactory.CreateClient();
            canvasHttpClient.BaseAddress = new Uri(base_url);
            canvasHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", canvas_api_key);

            var courses = await canvasHttpClient.GetFromJsonAsync<List<Course>>("/api/v1/users/self/favorites/courses");
            return Ok(courses.Select(c => c.course_code));
        }
    }

    public class Course
    {
        public int id { get; set; }
        public string name { get; set; }
        public string course_code { get; set; }
    }
}