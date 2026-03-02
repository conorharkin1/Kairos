using DashboardServer.Data;
using DashboardServer.Models.Constraints;
using DashboardServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DashboardServer.Repositories
{
    public class CourseWeightRepository : ICourseWeightRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CourseWeightRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<CourseWeight>> GetAllAsync()
        {
            return await _dbContext.CourseWeights.ToListAsync();
        }

        public async Task CreateBulkAsync(List<CourseWeight> weights)
        {
            await _dbContext.CourseWeights.AddRangeAsync(weights);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}