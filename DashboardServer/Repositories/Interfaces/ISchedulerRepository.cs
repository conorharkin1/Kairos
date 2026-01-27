using DashboardServer.Models;

namespace DashboardServer.Repositories.Interfaces
{
    public interface ISchedulerRepository
    {
        public Task<SchedulerData> GetDataForScheduler(string userId);
        public Task SaveSchedule(string userId, ScheduleBlock?[,] schedule);
        public Task<Schedule?> GetSchedule(string userId);
    }
}