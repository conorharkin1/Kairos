using DashboardServer.DTOs.ModelDTOs;
using DashboardServer.Models;

namespace DashboardServer.Services.Interfaces
{
    public interface ISchedulerService
    {
        Task RunScheduler();
        Task<ScheduleDTO?> GetSchedule();
    }
}