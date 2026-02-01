using DashboardServer.Models.Constraints;

namespace DashboardServer.Repositories.Interfaces
{
    public interface IAvoidPeriodRepository
    {
        public Task<List<AvoidPeriod>> GetAvoidPeriodsAsync();
        public Task<List<AvoidPeriod>> UpdateAvoidPeriods(List<AvoidPeriod> constraints, string userId);
    }
}