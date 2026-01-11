using DashboardServer.Models;

namespace DashboardServer.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        Task<Course> Create(string userId, Course course);
        Task CreateCoursesAsync(string userId, List<Course> courses);
        Task<List<Course>> GetCoursesAsync();
        Task<List<Assignment>> GetCourseAssignmentsAsync(int courseId, bool includeCompleted);
        Task<List<Quiz>> GetCourseQuizzesAsync(int courseId, bool includeCompleted);
    }
}