using DashboardServer.Models;

namespace DashboardServer.Repositories.Interfaces
{
    public interface IAssignmentRepository
    {
        Task<List<Assignment>> GetAllAsync();
        Task<List<Assignment>> GetAllCurrentAsync();
        Task<List<Assignment>> GetAllUpcomingUnsubmittedAsync();
        Task<Assignment?> GetByIdAsync(int id);
        Task<Assignment> CreateAsync(Assignment assignment);
        Task CreateBulkAsync(List<Assignment> assignments);
        Task DeleteAsync(Assignment assignment);

        Task Save();
    }
}