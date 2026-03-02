using DashboardServer.Models.Constraints;

namespace DashboardServer.Repositories.Interfaces
{
    public interface ISchedulerRulesRepository
    {
        Task<SchedulerRules?> GetAsync();
        Task<SchedulerRules> CreateAsync(SchedulerRules rules);
        Task Save();
    }
}