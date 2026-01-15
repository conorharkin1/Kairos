using DashboardServer.Data;
using DashboardServer.Models.Constraints;
using DashboardServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DashboardServer.Repositories
{
    public class SchedulerRulesRepository : ISchedulerRulesRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SchedulerRulesRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SchedulerRules?> GetRulesAsync()
        {
            var rules = await _dbContext.SchedulerRules.FirstOrDefaultAsync();
            return rules;
        }

        public async Task<SchedulerRules> UpdateRulesAsync(SchedulerRules rules, string userId)
        {
            var existingRules = await _dbContext.SchedulerRules.FirstOrDefaultAsync();
            
            if (existingRules == null)
            {
                rules.UserId = userId;
                var added = await _dbContext.SchedulerRules.AddAsync(rules);
                await _dbContext.SaveChangesAsync();
                return added.Entity;
            }

            existingRules.MinSessionLength = rules.MinSessionLength;
            existingRules.MaxSessionLength = rules.MaxSessionLength;
            await _dbContext.SaveChangesAsync();
            return existingRules;
        }
    }
}