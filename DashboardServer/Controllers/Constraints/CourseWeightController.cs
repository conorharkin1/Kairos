using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DashboardServer.Controllers.Constraints
{
    [ApiController]
    [Route("api/constaint/weight")]
    [Authorize]
    public class CourseWeightController: ControllerBase
    {
        public CourseWeightController()
        {
        }
    }
}