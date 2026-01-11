using DashboardServer.Models;

namespace DashboardServer.Services.Interfaces
{
    public interface ICourseService
    {
        public Task<Course> Create(string userId, Course course);
        public Task CreateCoursesAsync(string userId, List<Course> courses);
        public Task<List<Course>> GetCoursesAsync(string userId);
        public Task<List<Assignment>> GetCourseAssignmentsAsync(string userId, int courseId, bool includeCompleted);
        public Task<List<Quiz>> GetCourseQuizzesAsync(string userId, int courseId, bool includeCompleted);
    }
}