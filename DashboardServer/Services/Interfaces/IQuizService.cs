using DashboardServer.DTOs.ModelDTOs.Requests;
using DashboardServer.DTOs.ModelDTOs.Responses;

namespace DashboardServer.Services.Interfaces
{
    public interface IQuizService
    {
        Task<List<QuizResponseDTO>> GetAllAsync();
        Task<QuizResponseDTO> CreateAsync(string userId, QuizRequestDTO quiz);
        Task CreateBulkAsync(string userId, List<QuizRequestDTO> quizzes);
        Task<QuizResponseDTO> UpdateAsync(string userId, int quizId, QuizRequestDTO quiz);
        Task DeleteAsync(string userId, int quizId);
    }
}
