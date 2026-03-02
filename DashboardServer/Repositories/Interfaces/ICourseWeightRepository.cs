using DashboardServer.Models.Constraints;

namespace DashboardServer.Repositories.Interfaces
{
    public interface ICourseWeightRepository
    {
        Task<List<CourseWeight>> GetAllAsync();
        Task CreateBulkAsync(List<CourseWeight> weights);
        Task Save();
    }
}