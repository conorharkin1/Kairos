using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DashboardServer.Controllers.Constraints
{
    [ApiController]
    [Route("api/constaint/capacity")]
    [Authorize]
    public class DailyCapacityController: ControllerBase
    {
        public DailyCapacityController()
        {
        }
    }
}