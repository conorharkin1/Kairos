using DashboardServer.Exceptions;
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
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedException("User is not authorized");
            }

            return await _assignmentRepository.GetAll();
        }

        public async Task<Assignment> Create(string userId, Assignment assignment)
        {
            if (string.IsNullOrWhiteSpace(assignment.Name))
            {
                throw new BadRequestException("Assignment name cannot be empty");
            }

            return await _assignmentRepository.Create(userId, assignment);
        }

        public async Task CreateAssignmentsAsync(string userId, List<Assignment> assignments)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedException("User is not authorized");
            }

            await _assignmentRepository.CreateAssignmentsAsync(userId, assignments);
        }

        public async Task<Assignment> Update(string userId, int assignmentId, Assignment assignment)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedException("User is not authorized");
            }

            if (string.IsNullOrWhiteSpace(assignment.Name))
            {
                throw new BadRequestException("Assignment name cannot be empty");
            }

            return await _assignmentRepository.Update(userId, assignmentId, assignment);
        }

        public async Task Delete(string userId, int assignmentId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedException("User is not authorized");
            }

            await _assignmentRepository.Delete(userId, assignmentId);
        }
    }
}