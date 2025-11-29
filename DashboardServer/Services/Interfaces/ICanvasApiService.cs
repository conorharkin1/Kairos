using DashboardServer.DTOs;

namespace DashboardServer.Services.Interfaces
{
    public interface ICanvasApiService
    {
        Task<List<CanvasCourseDTO>> GetFavoriteCoursesFromCanvasAsync(string userId);
        Task<List<CanvasAssignmentDTO>> GetCourseAssignmentsFromCanvasAsync(string userId, int courseId);
        Task UpdateDataFromCanvasAsync(string userId);
    }
}