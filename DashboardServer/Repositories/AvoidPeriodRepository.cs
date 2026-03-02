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

        public async Task<List<AvoidPeriod>> GetAllAsync()
        {
            return await _dbContext.AvoidPeriods
                .Include(ap => ap.AvoidTimeBlocks)
                .OrderBy(ap => ap.DayIndex)
                .ToListAsync();
        }

        public async Task CreateBulkAsync(List<AvoidPeriod> avoidPeriods)
        {
            await _dbContext.AvoidPeriods.AddRangeAsync(avoidPeriods);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}