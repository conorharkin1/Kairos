using DashboardServer.Models;

namespace DashboardServer.Repositories.Interfaces
{
    public interface ISchedulerRepository
    {
        public Task<SchedulerData> GetSchedulerData(string userId);
    }
}