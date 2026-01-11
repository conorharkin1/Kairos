using DashboardServer.Models;

namespace DashboardServer.Repositories.Interfaces
{
    public interface IQuizRepository
    {
        Task<Quiz> Create(string userId, Quiz quiz);
        Task CreateQuizzesAsync(string userId, List<Quiz> quizzes);
        Task<Quiz> Update(string userId, int quizId, Quiz quiz);
        Task Delete(string userId, int quizId);
    }
}
