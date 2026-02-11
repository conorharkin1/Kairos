namespace DashboardServer.Controllers.Constraints
{
    using AutoMapper;
    using DashboardServer.DTOs.ConstraintDTOs;
    using DashboardServer.Exceptions;
    using DashboardServer.Models.Constraints;
    using DashboardServer.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http.HttpResults;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/constraint/avoidperiod")]
    [Authorize]
    public class AvoidPeriodController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAvoidPeriodService _service;

        public AvoidPeriodController(IMapper mapper, IAvoidPeriodService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<AvoidPeriodDTO>>> GetAvoidPeriods()
        {
            var periods = await _service.GetAvoidPeriodsAsync();
            return Ok(_mapper.Map<List<AvoidPeriodDTO>>(periods));
        }

        [HttpPut]
        public async Task<ActionResult<List<AvoidPeriodDTO>>> UpdateAvoidPeriodsAsync([FromBody] List<AvoidPeriodDTO> dtos)
        {
            var models = _mapper.Map<List<AvoidPeriod>>(dtos);

            var updated = await _service.UpdateAvoidPeriodsAsync(models);

            return Ok(_mapper.Map<List<AvoidPeriodDTO>>(updated));
        }
    }
}