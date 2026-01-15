using AutoMapper;
using DashboardServer.DTOs.ConstraintDTOs;
using DashboardServer.Models;
using DashboardServer.Models.Constraints;
using DashboardServer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DashboardServer.Controllers.Constraints
{
    [ApiController]
    [Route("api/constraint/rules")]
    [Authorize]
    public class SchedulerRulesController: ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISchedulerRulesService _service;
        
        public SchedulerRulesController(IMapper mapper, ISchedulerRulesService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        public async Task<SchedulerRulesDTO> GetSchedulerRulesAsync()
        {
            var rules = await _service.GetRulesAsync();
            return _mapper.Map<SchedulerRulesDTO>(rules);
        }

        [HttpPut]
        public async Task<SchedulerRulesDTO> UpdateSchedulerRulesAsync([FromBody] SchedulerRulesDTO rulesDto)
        {
            var rules = _mapper.Map<SchedulerRules>(rulesDto);
            var updatedRules = await _service.UpdateRulesAsync(rules);
            return _mapper.Map<SchedulerRulesDTO>(updatedRules);
        }
    }
}