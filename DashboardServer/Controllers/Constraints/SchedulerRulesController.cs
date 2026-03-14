using DashboardServer.DTOs.ConstraintDTOs.Requests;
using DashboardServer.DTOs.ConstraintDTOs.Responses;
using DashboardServer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DashboardServer.Controllers.Constraints
{
    [ApiController]
    [Route("api/constraint/rules")]
    [Authorize]
    public class SchedulerRulesController : ControllerBase
    {
        private readonly ISchedulerRulesService _service;

        public SchedulerRulesController(ISchedulerRulesService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<SchedulerRulesResponseDTO>> GetSchedulerRules()
        {
            var rules = await _service.GetAsync();
            return Ok(rules);
        }

        [HttpPut]
        public async Task<ActionResult<SchedulerRulesResponseDTO>> UpdateSchedulerRules([FromBody] SchedulerRulesRequestDTO dto)
        {
            var updated = await _service.UpdateAsync(dto);
            return Ok(updated);
        }

        [HttpPut("feedback")]
        public async Task<IActionResult> UpdateSchedulerPriorityCalculations([FromBody] SchedulerRulesRequestDTO dto)
        {
            await _service.UpdateSchedulerPrioritiesAsync(dto.DeadlineUrgencyWeighting, dto.CoursePreferenceWeighting);
            return Ok();
        }

    }
}