using DashboardServer.Models;

namespace DashboardServer.Repositories.Interfaces
{
    public interface IAssignmentRepository
    {
        Task<List<Assignment>> GetAll();
        Task<Assignment> Create(string userId, Assignment assignment);
        Task CreateAssignmentsAsync(string userId, List<Assignment> assignments);
        Task<Assignment> Update(string userId, int assignmentId, Assignment assignment);
        Task Delete(string userId, int assignmentId);
    }
}