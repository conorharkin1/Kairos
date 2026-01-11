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

        public async Task<Course> Create(string userId, Course course)
        {
            course.UserId = userId;
            var createdCourse = await _dbContext.Courses.AddAsync(course);
            await _dbContext.SaveChangesAsync();
            return createdCourse.Entity;
        }

        public async Task CreateCoursesAsync(string userId, List<Course> courses)
        {
            foreach (var course in courses)
            {
                course.UserId = userId;
            }

            await _dbContext.Courses.AddRangeAsync(courses);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Course>> GetCoursesAsync()
        {
            return await _dbContext.Courses.ToListAsync();
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