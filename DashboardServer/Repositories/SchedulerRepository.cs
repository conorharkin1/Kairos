using DashboardServer.Data;
using DashboardServer.Models;
using DashboardServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DashboardServer.Repositories
{
    public class SchedulerRepository : ISchedulerRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly ICourseRepository _courseRepository;

        public SchedulerRepository(ApplicationDbContext dbContext, IAssignmentRepository assignmentRepository, ICourseRepository courseRepository)
        {
            _dbContext = dbContext;
            _assignmentRepository = assignmentRepository;
            _courseRepository = courseRepository;
        }

        public async Task<SchedulerData> GetSchedulerData(string userId)
        {
            // Data
            var assignments = await _assignmentRepository.GetAllCurrent(userId);
            var courseIds = await _courseRepository.GetCourseIdsAsync();

            // Constraints
            var rules = await _dbContext.SchedulerRules.IgnoreQueryFilters().Where(a => a.UserId == userId).FirstOrDefaultAsync();
            var capacities = await _dbContext.DailyCapacities.IgnoreQueryFilters().Where(a => a.UserId == userId).ToListAsync();
            var weights = await _dbContext.CourseWeights.IgnoreQueryFilters().Where(a => a.UserId == userId).ToListAsync();

            return new SchedulerData {
                Assignments = assignments,
                CourseIds = courseIds,
                Rules = rules,
                DailyCapacities = capacities,
                CourseWeights = weights
            };
        }
    }
}