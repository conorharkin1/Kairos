using DashboardServer.DTOs.CanvasDTOs;
using DashboardServer.DTOs.ModelDTOs.Requests;
using DashboardServer.DTOs.ModelDTOs.Responses;
using DashboardServer.Models;

namespace DashboardServer.Services.Interfaces
{
    public interface IAssignmentService
    {
        public Task<List<AssignmentResponseDTO>> GetAllAsync();
        public Task<AssignmentResponseDTO> CreateAsync(string userId, AssignmentRequestDTO assignment);
        public Task CreateBulkAsync(string userId, List<AssignmentRequestDTO> assignments);
        public Task<AssignmentResponseDTO> UpdateAsync(string userId, int assignmentId, AssignmentRequestDTO assignment);
        public Task DeleteAsync(string userId, int assignmentId);
    }
}