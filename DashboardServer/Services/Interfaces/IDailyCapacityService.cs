using DashboardServer.Models.Constraints;

namespace DashboardServer.Services.Interfaces
{
    public interface IDailyCapacityService
    {
        Task<List<DailyCapacity>> GetDailyCapacitiesAsync();
        Task<List<DailyCapacity>> UpdateDailyCapacitiesAsync(List<DailyCapacity> capacities);
    }
}