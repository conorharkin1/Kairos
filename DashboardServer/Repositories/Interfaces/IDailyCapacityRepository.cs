using DashboardServer.Models.Constraints;

namespace DashboardServer.Repositories.Interfaces
{
    public interface IDailyCapacityRepository
    {
        public Task<List<DailyCapacity>> GetDailyCapacitiesAsync();
        public Task<List<DailyCapacity>> UpdateDailyCapacities(List<DailyCapacity> capacities, string userId);

    }
}