using DashboardServer.Models;

namespace DashboardServer.Services.Interfaces
{
    public interface IAssignmentService
    {
        public Task<List<Assignment>> GetAll(string userId);
        public Task<Assignment> Create(string userId, Assignment assignment);
        public Task<Assignment> Update(string userId, int assignmentId, Assignment assignment);
        public Task Delete(int assignmentId);
    }
}