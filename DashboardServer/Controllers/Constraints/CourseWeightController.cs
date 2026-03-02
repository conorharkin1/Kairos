using DashboardServer.DTOs.ConstraintDTOs.Requests;
using DashboardServer.DTOs.ConstraintDTOs.Responses;
using DashboardServer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DashboardServer.Controllers.Constraints
{
    [ApiController]
    [Route("api/constraint/weight")]
    [Authorize]
    public class CourseWeightController : ControllerBase
    {
        private readonly ICourseWeightService _service;

        public CourseWeightController(ICourseWeightService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<CourseWeightResponseDTO>>> GetCourseWeights()
        {
            var weights = await _service.GetAllAsync();
            return Ok(weights);
        }

        [HttpPut]
        public async Task<ActionResult<List<CourseWeightResponseDTO>>> UpdateCourseWeights([FromBody] List<CourseWeightRequestDTO> dtos)
        {
            var updated = await _service.UpdateAsync(dtos);
            return Ok(updated);
        }
    }
}