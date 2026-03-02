using DashboardServer.DTOs.ConstraintDTOs.Requests;
using DashboardServer.DTOs.ConstraintDTOs.Responses;

namespace DashboardServer.Services.Interfaces
{
    public interface ISchedulerRulesService
    {
        Task<SchedulerRulesResponseDTO> GetAsync();
        Task<SchedulerRulesResponseDTO> UpdateAsync(SchedulerRulesRequestDTO rules);
    }
}