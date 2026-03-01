using AutoMapper;
using DashboardServer.DTOs.CanvasDTOs;
using DashboardServer.DTOs.ModelDTOs.Requests;
using DashboardServer.DTOs.ModelDTOs.Responses;
using DashboardServer.Exceptions;
using DashboardServer.Models;
using DashboardServer.Repositories.Interfaces;
using DashboardServer.Services.Interfaces;

namespace DashboardServer.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IAssignmentRepository _repo;
        private readonly IMapper _mapper;
        public AssignmentService(IAssignmentRepository assignmentRepository, IMapper mapper)
        {
            _repo = assignmentRepository;
            _mapper = mapper;
        }

        public async Task<List<Assignment>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Assignment> CreateAsync(string userId, AssignmentRequestDTO dto)
        {
            Assignment assignment = new Assignment(userId, dto.CourseId, dto.Name, dto.Description, dto.DueDate, dto.HtmlUrl, dto.IsComplete, dto.IsPriority);

            return await _repo.CreateAsync(assignment);
        }

        public async Task CreateBulkAsync(string userId, List<AssignmentRequestDTO> assignments)
        {
            List<Assignment> assignmentsToCreate = assignments.Select(dto => new Assignment(userId, dto.CourseId, dto.Name, dto.Description, dto.DueDate, dto.HtmlUrl, dto.IsComplete, dto.IsPriority)).ToList();

            await _repo.CreateBulkAsync(assignmentsToCreate);
        }

        public async Task<AssignmentResponseDTO> UpdateAsync(string userId, int assignmentId, AssignmentRequestDTO dto)
        {
            Assignment assignment = await _repo.GetByIdAsync(assignmentId) ?? throw new NotFoundException("Assignment does not exist");

            if (assignment.UserId != userId)
            {
                throw new UnAuthorizedException("User is not authorized to update this assignment");
            }

            assignment.Update(dto.Name, dto.Description, dto.DueDate, dto.HtmlUrl, dto.IsComplete, dto.IsPriority);
            
            await _repo.Save();
            return _mapper.Map<AssignmentResponseDTO>(assignment);
        }

        public async Task DeleteAsync(string userId, int assignmentId)
        {
            var assignment = await _repo.GetByIdAsync(assignmentId) ?? throw new NotFoundException("Assignment does not exist");

            if (assignment.UserId != userId)
            {
                throw new UnAuthorizedException("User is not authorized to delete this assignment");
            }

            await _repo.DeleteAsync(assignment);
        }
    }
}