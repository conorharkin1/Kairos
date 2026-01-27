using AutoMapper;
using DashboardServer.DTOs.ConstraintDTOs;
using DashboardServer.Exceptions;
using DashboardServer.Models.Constraints;
using DashboardServer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DashboardServer.Controllers.Constraints
{
    [ApiController]
    [Route("api/constraint/capacity")]
    [Authorize]
    public class DailyCapacityController: ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDailyCapacityService _service;
        
        public DailyCapacityController(IMapper mapper, IDailyCapacityService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<DailyCapacityDTO>>> GetDailyCapacities()
        {
            var capacities = await _service.GetDailyCapacitiesAsync();
            return Ok(_mapper.Map<List<DailyCapacityDTO>>(capacities));
        }

        [HttpPut]
        public async Task<ActionResult<List<DailyCapacityDTO>>> UpdateDailyCapacitiesAsync([FromBody] List<DailyCapacityDTO> dtos)
        {
            var models = _mapper.Map<List<DailyCapacity>>(dtos);

            var updated = await _service.UpdateDailyCapacitiesAsync(models);

            return Ok(_mapper.Map<List<DailyCapacityDTO>>(updated));
        }
    }
}