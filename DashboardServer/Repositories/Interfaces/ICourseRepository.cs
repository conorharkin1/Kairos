using DashboardServer.Models;

namespace DashboardServer.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetCoursesAsync();
        Task<List<Assignment>> GetCourseAssignmentsAsync(int courseId);
        Task<List<Quiz>> GetCourseQuizzesAsync(int courseId);
    }
}