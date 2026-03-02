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

        public async Task<SchedulerRules?> GetAsync()
        {
            return await _dbContext.SchedulerRules.FirstOrDefaultAsync();
        }

        public async Task<SchedulerRules> CreateAsync(SchedulerRules rules)
        {
            var created = await _dbContext.SchedulerRules.AddAsync(rules);
            await _dbContext.SaveChangesAsync();
            return created.Entity;
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}