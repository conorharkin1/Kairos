using DashboardServer.DTOs;

namespace DashboardServer.Services.Interfaces
{
    public interface ICanvasApiService
    {
        Task<List<CanvasCourseDTO>> GetFavoriteCoursesAsync(string userId);
        Task<List<CanvasAssignmentDTO>> GetCourseAssignmentsAsync(string userId, int courseId);
    }
}