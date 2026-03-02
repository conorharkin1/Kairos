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

        public async Task<SchedulerData> GetDataForScheduler()
        {
            // Data
            var assignments = await _assignmentRepository.GetAllCurrentAsync();
            var courses = await _courseRepository.GetAllAsync();

            // Constraints
            var rules = await _dbContext.SchedulerRules.FirstOrDefaultAsync();
            var capacities = await _dbContext.DailyCapacities.ToListAsync();
            var weights = await _dbContext.CourseWeights.ToListAsync();
            var avoidPeriods = await _dbContext.AvoidPeriods.Include(ap => ap.AvoidTimeBlocks).ToListAsync();

            return new SchedulerData(
                assignments,
                courses,
                weights,
                capacities,
                rules,
                avoidPeriods
            );
        }

        public async Task SaveSchedule(string userId, ScheduleBlock?[,] scheduleMatrix)
        {
            // Firstly delete the existing schedule of the user
            var existing = await _dbContext.Schedules
                .Include(s => s.Blocks)
                .FirstOrDefaultAsync();
            
            if (existing != null)
            {
                _dbContext.Schedules.Remove(existing);
                await _dbContext.SaveChangesAsync();
            }

            var schedule = new Schedule(userId);

            int rows = scheduleMatrix.GetLength(0);
            int columns = scheduleMatrix.GetLength(1);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    var block = scheduleMatrix[row, col];
                    if (block != null)
                    {
                        block.SetSchedule(schedule);
                        schedule.Blocks.Add(block);
                    }
                }
            }

            _dbContext.Schedules.Add(schedule);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Schedule?> GetSchedule()
        {
            var schedule = await _dbContext.Schedules
                .Include(s => s.Blocks)
                .FirstOrDefaultAsync();

            return schedule;
        }
    }
}