using DashboardServer.Models;

namespace DashboardServer.Repositories.Interfaces
{
    public interface IAssignmentRepository
    {
        Task<Assignment> Create(string userId, Assignment assignment);
        Task<Assignment> Update(string userId, int assignmentId, Assignment assignment);
        Task Delete(int assignmentId);
    }
}