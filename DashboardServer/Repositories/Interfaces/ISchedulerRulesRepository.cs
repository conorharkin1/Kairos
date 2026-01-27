using DashboardServer.Models.Constraints;

namespace DashboardServer.Repositories.Interfaces
{
    public interface ISchedulerRulesRepository
    {
        public Task<SchedulerRules?> GetRulesAsync();
        public Task<SchedulerRules> UpdateRulesAsync(SchedulerRules rules, string userId);
    }
}