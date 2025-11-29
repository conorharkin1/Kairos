using DashboardServer.Models;

namespace DashboardServer.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetCoursesAsync(string userId);
        Task<List<Assignment>> GetCourseAssignmentsAsync(string userId, int courseId);
    }
}