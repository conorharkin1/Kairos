using DashboardServer.Models.Constraints;

namespace DashboardServer.Repositories.Interfaces
{
    public interface IDailyCapacityRepository
    {
        Task<List<DailyCapacity>> GetAllAsync();
        Task CreateBulkAsync(List<DailyCapacity> capacities);
        Task Save();
    }
}