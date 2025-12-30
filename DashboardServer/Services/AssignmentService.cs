using DashboardServer.Models;
using DashboardServer.Repositories.Interfaces;
using DashboardServer.Services.Interfaces;

namespace DashboardServer.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IAssignmentRepository _assignmentRepository;
        public AssignmentService(IAssignmentRepository assignmentRepository)
        {
            _assignmentRepository = assignmentRepository;
        }

        public async Task<List<Assignment>> GetAll(string userId)
        {
            return await _assignmentRepository.GetAll(userId);
        }

        public async Task<Assignment> Create(string userId, Assignment assignment)
        {
            return await _assignmentRepository.Create(userId, assignment);
        }

        public async Task<Assignment> Update(string userId, int assignmentId, Assignment assignment)
        {
            return await _assignmentRepository.Update(userId, assignmentId, assignment);
        }

        public async Task Delete(int assignmentId)
        {
            await _assignmentRepository.Delete(assignmentId);
        }
    }
}