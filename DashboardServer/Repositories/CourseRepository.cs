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

        public async Task<List<Course>> GetAllAsync()
        {
            return await _dbContext.Courses.AsNoTracking().ToListAsync();
        }

        public async Task<Course?> GetByIdAsync(int id)
        {
            return await _dbContext.Courses.FindAsync(id);
        }

        public async Task<Course> CreateAsync(Course course)
        {
            var createdCourse = await _dbContext.Courses.AddAsync(course);
            await _dbContext.SaveChangesAsync();
            return createdCourse.Entity;
        }

        public async Task CreateBulkAsync(List<Course> courses)
        {
            await _dbContext.Courses.AddRangeAsync(courses);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Course course)
        {
            _dbContext.Courses.Remove(course);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<int>> GetCourseIdsAsync()
        {
            return await _dbContext.Courses.Select(c => c.Id).ToListAsync();
        }

        public async Task<List<Assignment>> GetCourseAssignmentsAsync(int courseId, bool includeCompleted)
        {
            if (includeCompleted)
            {
                return await _dbContext.Assignments.Where(a => a.CourseId == courseId).ToListAsync();
            }
            else
            {
                return await _dbContext.Assignments.Where(a => a.CourseId == courseId && a.DueDate >= DateTime.UtcNow).ToListAsync();
            }
        }

        public async Task<List<Quiz>> GetCourseQuizzesAsync(int courseId, bool includeCompleted)
        {
            if (includeCompleted)
            {
                return await _dbContext.Quizzes.Where(q => q.CourseId == courseId).ToListAsync();
            }
            else
            {
                return await _dbContext.Quizzes.Where(q => q.CourseId == courseId && q.DueDate >= DateTime.UtcNow).ToListAsync();
            }
        }
    }
}