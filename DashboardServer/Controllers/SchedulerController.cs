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
        public async Task<IActionResult> RunScheduler()
        {
            await _schedulerService.RunScheduler();
            return Ok(new { message = "Schedule generated successfully." });
        }

        [HttpGet("schedule")]
        public async Task<IActionResult> GetSchedule()
        {
            var schedule = await _schedulerService.GetSchedule();
            return Ok(schedule);
        }
    }
}