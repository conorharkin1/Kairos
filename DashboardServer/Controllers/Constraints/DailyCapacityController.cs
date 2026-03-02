using DashboardServer.DTOs.ConstraintDTOs.Requests;
using DashboardServer.DTOs.ConstraintDTOs.Responses;
using DashboardServer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DashboardServer.Controllers.Constraints
{
    [ApiController]
    [Route("api/constraint/capacity")]
    [Authorize]
    public class DailyCapacityController : ControllerBase
    {
        private readonly IDailyCapacityService _service;

        public DailyCapacityController(IDailyCapacityService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<DailyCapacityResponseDTO>>> GetDailyCapacities()
        {
            var capacities = await _service.GetAllAsync();
            return Ok(capacities);
        }

        [HttpPut]
        public async Task<ActionResult<List<DailyCapacityResponseDTO>>> UpdateDailyCapacities([FromBody] List<DailyCapacityRequestDTO> dtos)
        {
            var updated = await _service.UpdateAsync(dtos);
            return Ok(updated);
        }
    }
}