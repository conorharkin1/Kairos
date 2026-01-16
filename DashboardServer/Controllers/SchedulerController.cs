namespace DashboardServer.Controllers
{
    using DashboardServer.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SchedulerController : ControllerBase
    {
        private readonly ISchedulerService _schedulerService;

        public SchedulerController(ISchedulerService schedulerService)
        {
            _schedulerService = schedulerService;
        }

        [HttpPost]
        public IActionResult RunScheduler()
        {
            _schedulerService.RunScheduler();
            return Ok(new { message = "Scheduler started successfully." });
        }
    }
}