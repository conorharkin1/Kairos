using DashboardServer.Models;

namespace DashboardServer.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetAllAsync();
        Task<Course?> GetByIdAsync(int id);
        Task<Course> CreateAsync(Course course);
        Task CreateBulkAsync(List<Course> courses);
        Task DeleteAsync(Course course);
        Task Save();
        Task<List<int>> GetCourseIdsAsync();
        Task<List<Assignment>> GetCourseAssignmentsAsync(int courseId, bool includeCompleted);
        Task<List<Quiz>> GetCourseQuizzesAsync(int courseId, bool includeCompleted);
    }
}