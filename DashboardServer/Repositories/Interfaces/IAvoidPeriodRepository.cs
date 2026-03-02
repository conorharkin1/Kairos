using DashboardServer.Models.Constraints;

namespace DashboardServer.Repositories.Interfaces
{
    public interface IAvoidPeriodRepository
    {
        Task<List<AvoidPeriod>> GetAllAsync();
        Task CreateBulkAsync(List<AvoidPeriod> avoidPeriods);
        Task Save();
    }
}