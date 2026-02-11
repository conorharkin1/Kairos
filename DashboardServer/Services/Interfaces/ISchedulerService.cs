using DashboardServer.DTOs.ModelDTOs;
using DashboardServer.Models;

namespace DashboardServer.Services.Interfaces
{
    public interface ISchedulerService
    {
        public Task RunScheduler();
        public Task<ScheduleDTO?> GetSchedule(string userId);
    }
}