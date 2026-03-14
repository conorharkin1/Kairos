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
        private readonly IExportService _exportService;

        public SchedulerController(ISchedulerService schedulerService, IExportService exportService)
        {
            _schedulerService = schedulerService;
            _exportService = exportService;
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

        [HttpGet("export")]
        public async Task<IActionResult> ExportSchedule()
        {
            var weeklySchedule = await _exportService.GetScheduleAsIcsAsync();
            return File(weeklySchedule, "text/calendar");
        }
    }
}