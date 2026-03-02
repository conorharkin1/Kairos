using DashboardServer.DTOs.ConstraintDTOs.Requests;
using DashboardServer.DTOs.ConstraintDTOs.Responses;
using DashboardServer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DashboardServer.Controllers.Constraints
{
    [ApiController]
    [Route("api/constraint/avoidperiod")]
    [Authorize]
    public class AvoidPeriodController : ControllerBase
    {
        private readonly IAvoidPeriodService _service;

        public AvoidPeriodController(IAvoidPeriodService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<AvoidPeriodResponseDTO>>> GetAvoidPeriods()
        {
            var periods = await _service.GetAllAsync();
            return Ok(periods);
        }

        [HttpPut]
        public async Task<ActionResult<List<AvoidPeriodResponseDTO>>> UpdateAvoidPeriods([FromBody] List<AvoidPeriodRequestDTO> dtos)
        {
            var updated = await _service.UpdateAsync(dtos);
            return Ok(updated);
        }
    }
}