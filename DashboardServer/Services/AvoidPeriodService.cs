using DashboardServer.Exceptions;
using DashboardServer.Models;
using DashboardServer.Models.Constraints;
using DashboardServer.Repositories.Interfaces;
using DashboardServer.Services.Interfaces;

namespace DashboardServer.Services
{
    public class AvoidPeriodService : IAvoidPeriodService
    {
        private readonly IAvoidPeriodRepository _repository;
        private readonly IUserResolverService _userResolverService;
        public AvoidPeriodService(IUserResolverService userResolverService, IAvoidPeriodRepository repository)
        {
            _userResolverService = userResolverService;
            _repository = repository;
        }

        private string? CURRENT_USER_ID => _userResolverService.UserId;

        public async Task<List<AvoidPeriod>> GetAvoidPeriodsAsync()
        {
            var constraints = await _repository.GetAvoidPeriodsAsync();
            if (constraints.Count == 0)
            {
                return CreateDefaultAvoidPeriods();
            }
            constraints = constraints.OrderBy(c => c.DayIndex).ToList();
            return constraints;
        }

        public async Task<List<AvoidPeriod>> UpdateAvoidPeriodsAsync(List<AvoidPeriod> constraints)
        {
            if (constraints.Count != 7)
            {
                throw new ValidationException("There can only be 7 avoid period constraints, one for each day of the week.");
            }

            var isInvalidIndex = constraints.Any(c => c.DayIndex < 0 || c.DayIndex > 6);
            if (isInvalidIndex)
            {
                throw new ValidationException("Day indices must be between 0 and 6.");
            }

            var updated = await _repository.UpdateAvoidPeriods(constraints, CURRENT_USER_ID!);
            updated = updated.OrderBy(c => c.DayIndex).ToList();
            return updated;
        }

        private List<AvoidPeriod> CreateDefaultAvoidPeriods()
        {
            return Enumerable.Range(0, 7)
                .Select(index => new AvoidPeriod
                {
                    UserId = CURRENT_USER_ID!,
                    DayIndex = index,
                    AvoidTimeBlocks = new List<AvoidTimeBlock>()
                })
                .ToList();
        }
    }
}