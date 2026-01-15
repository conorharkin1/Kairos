using DashboardServer.Data;
using DashboardServer.Models.Constraints;
using DashboardServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DashboardServer.Repositories
{
    public class CourseWeightRepository: ICourseWeightRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CourseWeightRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<CourseWeight>> GetCourseWeightsAsync()
        {
            return await _dbContext.CourseWeights.ToListAsync();
        }

        public async Task<List<CourseWeight>> UpdateCourseWeights(List<CourseWeight> weights, string userId)
        {
            var weightIds = weights.Select(w => w.Id).ToList();
            var existingWeights = await _dbContext.CourseWeights.Where(w => weightIds.Contains(w.Id)).ToDictionaryAsync(w => w.Id);

            var results = new List<CourseWeight>();
            foreach (var weight in weights)
            {
                if (existingWeights.TryGetValue(weight.Id, out var existingWeight))
                {
                    existingWeight.Weight = weight.Weight;
                    results.Add(existingWeight);
                }
                else
                {
                    weight.UserId = userId;
                    var added = _dbContext.CourseWeights.Add(weight);
                    results.Add(added.Entity);
                }
            }

            await _dbContext.SaveChangesAsync();
            return results;
        }
    }
}