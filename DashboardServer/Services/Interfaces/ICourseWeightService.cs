using DashboardServer.DTOs.ConstraintDTOs.Requests;
using DashboardServer.DTOs.ConstraintDTOs.Responses;

namespace DashboardServer.Services.Interfaces
{
    public interface ICourseWeightService
    {
        Task<List<CourseWeightResponseDTO>> GetAllAsync();
        Task<List<CourseWeightResponseDTO>> UpdateAsync(List<CourseWeightRequestDTO> weights);
    }
}