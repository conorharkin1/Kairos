using DashboardServer.Models;

namespace DashboardServer.Services.Interfaces
{
    public interface IAssignmentService
    {
        public Task<List<Assignment>> GetAll(string userId);
        public Task<Assignment> Create(string userId, Assignment assignment);
        public Task CreateAssignmentsAsync(string userId, List<Assignment> assignments);
        public Task<Assignment> Update(string userId, int assignmentId, Assignment assignment);
        public Task Delete(string userId, int assignmentId);
    }
}