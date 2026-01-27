namespace DashboardServer.Controllers
{
    using DashboardServer.Exceptions;
    using DashboardServer.Models;
    using DashboardServer.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SchedulerController : ControllerBase
    {
        private readonly ISchedulerService _schedulerService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SchedulerController(ISchedulerService schedulerService, UserManager<ApplicationUser> userManager)
        {
            _schedulerService = schedulerService;
            _userManager = userManager;
        }

        [HttpPost]
        public IActionResult RunScheduler()
        {
            _schedulerService.RunScheduler();
            return Ok(new { message = "Scheduler started successfully." });
        }

        [HttpGet("schedule")]
        public async Task<IActionResult> GetSchedule()
        {
            string userId = _userManager.GetUserId(HttpContext.User)!;
            
            var schedule = await _schedulerService.GetSchedule(userId);
            return Ok(schedule);
        }
    }
}