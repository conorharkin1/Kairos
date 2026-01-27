using DashboardServer.Exceptions;
using DashboardServer.Models.Constraints;
using DashboardServer.Repositories.Interfaces;
using DashboardServer.Services.Interfaces;

namespace DashboardServer.Services
{
    public class SchedulerRulesService : ISchedulerRulesService
    {
        private readonly ISchedulerRulesRepository _repository;
        private readonly IUserResolverService _userResolverService;

        public SchedulerRulesService(ISchedulerRulesRepository repository, IUserResolverService userResolverService)
        {
            _repository = repository;
            _userResolverService = userResolverService;
        }

        private string? CURRENT_USER_ID => _userResolverService.UserId;

        public async Task<SchedulerRules> GetRulesAsync()
        {
            var rules = await _repository.GetRulesAsync();
            if (rules == null)
            {
                rules = new SchedulerRules
                {
                    MinSessionLength = 30,
                    MaxSessionLength = 180,
                };
            }
            return rules;
        }

        public async Task<SchedulerRules> UpdateRulesAsync(SchedulerRules rules)
        {
            if(rules.MinSessionLength <= 0 || rules.MaxSessionLength <= 0)
            {
                throw new ValidationException("Session lengths must be positive integers.");
            }
            if(rules.MinSessionLength > rules.MaxSessionLength)
            {
                throw new ValidationException("Minimum session length cannot be greater than maximum session length.");
            }
            
            var updated = await _repository.UpdateRulesAsync(rules, CURRENT_USER_ID!);
            return updated;
        }
    }
}