using DashboardServer.Models;

namespace DashboardServer.Repositories.Interfaces
{
    public interface IAssignmentRepository
    {
        Task<List<Assignment>> GetAll(string userId);
        Task<Assignment> Create(string userId, Assignment assignment);
        Task<Assignment> Update(string userId, int assignmentId, Assignment assignment);
        Task Delete(int assignmentId);
    }
}