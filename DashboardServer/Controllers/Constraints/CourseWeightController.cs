using AutoMapper;
using DashboardServer.DTOs.ConstraintDTOs;
using DashboardServer.Models.Constraints;
using DashboardServer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DashboardServer.Controllers.Constraints
{
    [ApiController]
    [Route("api/constraint/weight")]
    [Authorize]
    public class CourseWeightController: ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICourseWeightService _service;
        
        public CourseWeightController(IMapper mapper, ICourseWeightService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<CourseWeightDTO>>> GetCourseWeightsAsync()
        {
            var weights = await _service.GetCourseWeightsAsync();
            return Ok(_mapper.Map<List<CourseWeightDTO>>(weights));
        }

        [HttpPut]
        public async Task<ActionResult<List<CourseWeightDTO>>> UpdateCourseWeightsAsync([FromBody] List<CourseWeightDTO> dtos)
        {
            var models = _mapper.Map<List<CourseWeight>>(dtos);

            var updated = await _service.UpdateCourseWeightsAsync(models);
            
            return Ok(_mapper.Map<List<CourseWeightDTO>>(updated));
        }
    }
}