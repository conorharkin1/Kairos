using DashboardServer.DTOs.ConstraintDTOs.Requests;
using DashboardServer.DTOs.ConstraintDTOs.Responses;

namespace DashboardServer.Services.Interfaces
{
    public interface IAvoidPeriodService
    {
        Task<List<AvoidPeriodResponseDTO>> GetAllAsync();
        Task<List<AvoidPeriodResponseDTO>> UpdateAsync(List<AvoidPeriodRequestDTO> avoidPeriods);
    }
}