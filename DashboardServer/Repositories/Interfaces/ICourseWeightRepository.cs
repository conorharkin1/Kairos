using DashboardServer.Models.Constraints;

namespace DashboardServer.Repositories.Interfaces
{
    public interface ICourseWeightRepository
    {
        public Task<List<CourseWeight>> GetCourseWeightsAsync();
        public Task<List<CourseWeight>> UpdateCourseWeights(List<CourseWeight> weights, string userId);
    }
}