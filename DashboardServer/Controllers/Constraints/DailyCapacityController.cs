using AutoMapper;
using DashboardServer.DTOs.ConstraintDTOs;
using DashboardServer.Models.Constraints;
using DashboardServer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DashboardServer.Controllers.Constraints
{
    [ApiController]
    [Route("api/constraint/capacity")]
    [Authorize]
    public class DailyCapacityController: ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGenericConstraintService<DailyCapacity> _service;
        
        public DailyCapacityController(IMapper mapper, IGenericConstraintService<DailyCapacity> service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        public async Task<List<DailyCapacityDTO>> GetDailyCapacitiesAsync()
        {
            var capacities = await _service.GetListConstraintsAsync();
            capacities = capacities.OrderBy(c => c.DayIndex).ToList();
            
            return _mapper.Map<List<DailyCapacityDTO>>(capacities);
        }

        [HttpPut]
        public async Task<List<DailyCapacityDTO>> UpdateDailyCapacitiesAsync([FromBody] List<DailyCapacityDTO> dtos)
        {
            var model = _mapper.Map<List<DailyCapacity>>(dtos);
            var updatedConstraints = await _service.UpdateListConstraintsAsync(model);

            updatedConstraints = updatedConstraints.OrderBy(c => c.DayIndex).ToList();
            return _mapper.Map<List<DailyCapacityDTO>>(updatedConstraints);
        }
    }
}