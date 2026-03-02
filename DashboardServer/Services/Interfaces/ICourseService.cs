using DashboardServer.DTOs.ModelDTOs.Requests;
using DashboardServer.DTOs.ModelDTOs.Responses;

namespace DashboardServer.Services.Interfaces
{
    public interface ICourseService
    {
        Task<List<CourseResponseDTO>> GetAllAsync();
        Task<CourseResponseDTO> CreateAsync(string userId, CourseRequestDTO course);
        Task CreateBulkAsync(string userId, List<CourseRequestDTO> courses);
        Task<CourseResponseDTO> UpdateAsync(string userId, int courseId, CourseRequestDTO course);
        Task DeleteAsync(string userId, int courseId);
        Task<List<AssignmentResponseDTO>> GetCourseAssignmentsAsync(int courseId, bool includeCompleted);
        Task<List<QuizResponseDTO>> GetCourseQuizzesAsync(int courseId, bool includeCompleted);
    }
}