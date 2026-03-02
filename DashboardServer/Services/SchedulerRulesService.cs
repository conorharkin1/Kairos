using AutoMapper;
using DashboardServer.DTOs.ConstraintDTOs.Requests;
using DashboardServer.DTOs.ConstraintDTOs.Responses;
using DashboardServer.Models.Constraints;
using DashboardServer.Repositories.Interfaces;
using DashboardServer.Services.Interfaces;

namespace DashboardServer.Services
{
    public class SchedulerRulesService : ISchedulerRulesService
    {
        private readonly ISchedulerRulesRepository _repo;
        private readonly IMapper _mapper;
        private readonly IUserResolverService _userResolverService;

        public SchedulerRulesService(ISchedulerRulesRepository repository, IMapper mapper, IUserResolverService userResolverService)
        {
            _repo = repository;
            _mapper = mapper;
            _userResolverService = userResolverService;
        }

        private string CurrentUserId => _userResolverService.UserId!;

        public async Task<SchedulerRulesResponseDTO> GetAsync()
        {
            var rules = await _repo.GetAsync();
            if (rules == null)
            {
                rules = new SchedulerRules(CurrentUserId);
                rules = await _repo.CreateAsync(rules);
            }

            return _mapper.Map<SchedulerRulesResponseDTO>(rules);
        }

        public async Task<SchedulerRulesResponseDTO> UpdateAsync(SchedulerRulesRequestDTO dto)
        {
            var rules = await _repo.GetAsync();
            if (rules == null)
            {
                rules = new SchedulerRules(CurrentUserId, dto.MinSessionLength, dto.MaxSessionLength);
                rules = await _repo.CreateAsync(rules);
            }
            else
            {
                rules.Update(dto.MinSessionLength, dto.MaxSessionLength);
                await _repo.Save();
            }

            return _mapper.Map<SchedulerRulesResponseDTO>(rules);
        }
    }
}