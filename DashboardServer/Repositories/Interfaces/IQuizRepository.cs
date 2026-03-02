using DashboardServer.Models;

namespace DashboardServer.Repositories.Interfaces
{
    public interface IQuizRepository
    {
        Task<List<Quiz>> GetAllAsync();
        Task<Quiz?> GetByIdAsync(int id);
        Task<Quiz> CreateAsync(Quiz quiz);
        Task CreateBulkAsync(List<Quiz> quizzes);
        Task DeleteAsync(Quiz quiz);
        Task Save();
    }
}
