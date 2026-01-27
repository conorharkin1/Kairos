using DashboardServer.Models.Constraints;
using DashboardServer.Repositories.Interfaces;

namespace DashboardServer.Services.Interfaces
{
    public interface ICourseWeightService
    {
        Task<List<CourseWeight>> GetCourseWeightsAsync();
        Task<List<CourseWeight>> UpdateCourseWeightsAsync(List<CourseWeight> weights);
    }
}