using DashboardServer.Data;
using DashboardServer.Models;
using DashboardServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DashboardServer.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CourseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Course>> GetCoursesAsync(string userId)
        {
            return await _dbContext.Courses.ToListAsync();
        }

        public async Task<List<Assignment>> GetCourseAssignmentsAsync(string userId, int courseId)
        {
            return await _dbContext.Assignments.Where(a => a.CourseId == courseId).ToListAsync();
        }

        public async Task<List<Quiz>> GetCourseQuizzesAsync(string userId, int courseId)
        {
            return await _dbContext.Quizzes.Where(q => q.CourseId == courseId).ToListAsync();
        }
    }
}