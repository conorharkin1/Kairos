using DashboardServer.Models.Constraints;

namespace DashboardServer.Services.Interfaces
{
    public interface IAvoidPeriodService
    {
        Task<List<AvoidPeriod>> GetAvoidPeriodsAsync();
        Task<List<AvoidPeriod>> UpdateAvoidPeriodsAsync(List<AvoidPeriod> constraints);
    }
}