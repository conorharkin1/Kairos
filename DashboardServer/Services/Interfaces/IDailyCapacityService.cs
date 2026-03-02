using DashboardServer.DTOs.ConstraintDTOs.Requests;
using DashboardServer.DTOs.ConstraintDTOs.Responses;

namespace DashboardServer.Services.Interfaces
{
    public interface IDailyCapacityService
    {
        Task<List<DailyCapacityResponseDTO>> GetAllAsync();
        Task<List<DailyCapacityResponseDTO>> UpdateAsync(List<DailyCapacityRequestDTO> capacities);
    }
}