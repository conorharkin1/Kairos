using DashboardServer.Data;
using DashboardServer.Models.Constraints;
using DashboardServer.Repositories.Interfaces;
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

        public async Task<List<DailyCapacity>> GetAllAsync()
        {
            return await _dbContext.DailyCapacities.OrderBy(c => c.DayIndex).ToListAsync();
        }

        public async Task CreateBulkAsync(List<DailyCapacity> capacities)
        {
            await _dbContext.DailyCapacities.AddRangeAsync(capacities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}