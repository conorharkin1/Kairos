
using System.ComponentModel.DataAnnotations;
using DashboardServer.Models.Constraints;
using DashboardServer.Repositories.Interfaces;
using DashboardServer.Services.Interfaces;

namespace DashboardServer.Services
{
    public class DailyCapacityService : IDailyCapacityService
    {
        private readonly IDailyCapacityRepository _repository;
        private readonly IUserResolverService _userResolverService;

        public DailyCapacityService(IDailyCapacityRepository repository, IUserResolverService userResolverService)
        {
            _repository = repository;
            _userResolverService = userResolverService;
        }

        private string? CURRENT_USER_ID => _userResolverService.UserId;

        public async Task<List<DailyCapacity>> GetDailyCapacitiesAsync()
        {
            var constraints = await _repository.GetDailyCapacitiesAsync();
            if (constraints.Count == 0)
            {
                return CreateDefaultDailyCapacities();
            }
            constraints = constraints.OrderBy(c => c.DayIndex).ToList();
            return constraints;
        }

        public async Task<List<DailyCapacity>> UpdateDailyCapacitiesAsync(List<DailyCapacity> constraints)
        {
            if (constraints.Count != 7)
            {
                throw new ValidationException("There must be exactly 7 daily capacities, one for each day of the week.");
            }

            var isInvalidIndex = constraints.Any(c => c.DayIndex < 0 || c.DayIndex > 6);
            if (isInvalidIndex)
            {
                throw new ValidationException("Day indices must be between 0 and 6.");
            }

            var updated = await _repository.UpdateDailyCapacities(constraints, CURRENT_USER_ID!);
            updated = updated.OrderBy(c => c.DayIndex).ToList();
            return updated;
        }

        private List<DailyCapacity> CreateDefaultDailyCapacities()
        {
            return Enumerable.Range(0, 7)
                .Select(index => new DailyCapacity
                {
                    UserId = CURRENT_USER_ID!,
                    DayIndex = index,
                    MaxHours = 0
                })
                .ToList();
        }
    }
}