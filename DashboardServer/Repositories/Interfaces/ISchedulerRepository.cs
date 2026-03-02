using DashboardServer.Models;

namespace DashboardServer.Repositories.Interfaces
{
    public interface ISchedulerRepository
    {
        Task<SchedulerData> GetDataForScheduler();
        Task SaveSchedule(string userId, ScheduleBlock?[,] schedule);
        Task<Schedule?> GetSchedule();
    }
}