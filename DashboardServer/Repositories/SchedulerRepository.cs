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

        public async Task<SchedulerData> GetDataForScheduler(string userId)
        {
            // Data
            var assignments = await _assignmentRepository.GetAllCurrent(userId);
            var courses = await _courseRepository.GetCoursesAsync(userId);

            // Constraints
            var rules = await _dbContext.SchedulerRules.IgnoreQueryFilters().Where(a => a.UserId == userId).FirstOrDefaultAsync();
            var capacities = await _dbContext.DailyCapacities.IgnoreQueryFilters().Where(a => a.UserId == userId).ToListAsync();
            var weights = await _dbContext.CourseWeights.IgnoreQueryFilters().Where(a => a.UserId == userId).ToListAsync();

            return new SchedulerData {
                Assignments = assignments,
                Courses = courses,
                Rules = rules,
                DailyCapacities = capacities,
                CourseWeights = weights
            };
        }

        public async Task SaveSchedule(string userId, ScheduleBlock?[,] scheduleMatrix)
        {
            // Firstly delete the existing schedule of the user
            var existing = await _dbContext.Schedules
                .Include(s => s.Blocks)
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(s => s.UserId == userId);
            
            if (existing != null)
            {
                _dbContext.Schedules.Remove(existing);
                await _dbContext.SaveChangesAsync();
            }

            var schedule = new Schedule
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            int rows = scheduleMatrix.GetLength(0);
            int columns = scheduleMatrix.GetLength(1);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    var block = scheduleMatrix[row, col];
                    if (block != null)
                    {
                        block.Schedule = schedule;
                        schedule.Blocks.Add(block);
                    }
                }
            }

            _dbContext.Schedules.Add(schedule);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Schedule?> GetSchedule(string userId)
        {
            var schedule = await _dbContext.Schedules
                .Include(s => s.Blocks)
                .FirstOrDefaultAsync(s => s.UserId == userId);

            return schedule;
        }
    }
}