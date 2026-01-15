using DashboardServer.Models.Constraints;

namespace DashboardServer.Services.Interfaces
{
    public interface ISchedulerRulesService
    {
        Task<SchedulerRules> GetRulesAsync();
        Task<SchedulerRules> UpdateRulesAsync(SchedulerRules rules);
    }
}