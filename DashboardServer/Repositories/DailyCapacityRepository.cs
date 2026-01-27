using DashboardServer.Data;
using DashboardServer.Models.Constraints;
using DashboardServer.Repositories.Interfaces;
using DashboardServer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DashboardServer.Repositories
{
    public class DailyCapacityRepository : IDailyCapacityRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DailyCapacityRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<DailyCapacity>> GetDailyCapacitiesAsync()
        {
            return await _dbContext.DailyCapacities.ToListAsync();
        }

        public async Task<List<DailyCapacity>> UpdateDailyCapacities(List<DailyCapacity> constraints, string userId)
        {
            var constraintIds = constraints.Select(c => c.Id).ToList();
            var existingConstraints = await _dbContext.DailyCapacities.Where(c => constraintIds.Contains(c.Id)).ToDictionaryAsync(c => c.Id);
            var results = new List<DailyCapacity>();
            foreach (var constraint in constraints)
            {
                if (existingConstraints.TryGetValue(constraint.Id, out var existingConstraint))
                {
                    existingConstraint.DayIndex = constraint.DayIndex;
                    existingConstraint.MaxHours = constraint.MaxHours;
                    results.Add(existingConstraint);
                }
                else
                {
                    constraint.UserId = userId;
                    var added = _dbContext.DailyCapacities.Add(constraint);
                    results.Add(added.Entity);
                }
            }

            await _dbContext.SaveChangesAsync();
            return results;
        }
    }
}