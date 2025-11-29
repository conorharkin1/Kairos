using DashboardServer.Models;

namespace DashboardServer.Services.Interfaces
{
    public interface ICourseService
    {
        public Task<List<Course>> GetCoursesAsync(string userId);
        public Task<List<Assignment>> GetCourseAssignmentsAsync(string userId, int courseId);
    }
}