using DashboardServer.Data;
using DashboardServer.Models.Constraints;
using DashboardServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DashboardServer.Repositories
{
    public class AvoidPeriodRepository : IAvoidPeriodRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AvoidPeriodRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<AvoidPeriod>> GetAvoidPeriodsAsync()
        {
            return await _dbContext.AvoidPeriods.Include(ap => ap.AvoidTimeBlocks).ToListAsync();
        }

        public async Task<List<AvoidPeriod>> UpdateAvoidPeriods(List<AvoidPeriod> constraints, string userId)
        {
            var constraintIds = constraints.Select(c => c.Id).ToList();
            var existingConstraints = await _dbContext.AvoidPeriods.Include(ap => ap.AvoidTimeBlocks).Where(c => constraintIds.Contains(c.Id)).ToDictionaryAsync(c => c.Id);
            var results = new List<AvoidPeriod>();
            foreach (var constraint in constraints)
            {
                if (existingConstraints.TryGetValue(constraint.Id, out var existingConstraint))
                {
                    existingConstraint.DayIndex = constraint.DayIndex;
                    existingConstraint.AvoidTimeBlocks = constraint.AvoidTimeBlocks;
                    results.Add(existingConstraint);
                }
                else
                {
                    constraint.UserId = userId;
                    var added = _dbContext.AvoidPeriods.Add(constraint);
                    results.Add(added.Entity);
                }
            }

            await _dbContext.SaveChangesAsync();
            return results;
        }
    }
}