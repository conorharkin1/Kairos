using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DashboardServer.Controllers.Constraints
{
    [ApiController]
    [Route("api/constaint/rules")]
    [Authorize]
    public class SchedulerRulesController: ControllerBase
    {
        public SchedulerRulesController()
        {
        }
    }
}