using Microsoft.AspNetCore.Mvc;

namespace DashboardServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        [HttpGet("courses")]
        public IActionResult GetCourses()
        {
            return Ok(new[] { "Course 1", "Course 2" });
        }
    }
}