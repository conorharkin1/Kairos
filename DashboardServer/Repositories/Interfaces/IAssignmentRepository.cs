using DashboardServer.Models;

namespace DashboardServer.Repositories.Interfaces
{
    public interface IAssignmentRepository
    {
        Task<List<Assignment>> GetAllAsync();
        Task<List<Assignment>> GetAllCurrentAsync();
        Task<Assignment?> GetByIdAsync(int id);
        Task<Assignment> CreateAsync(Assignment assignment);
        Task CreateBulkAsync(List<Assignment> assignments);
        Task DeleteAsync(Assignment assignment);

        Task Save();
    }
}