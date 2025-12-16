using DashboardServer.Models;

namespace DashboardServer.Repositories.Interfaces
{
    public interface IQuizRepository
    {
        Task<Quiz> Create(string userId, Quiz quiz);
        Task<Quiz> Update(string userId, int quizId, Quiz quiz);
        Task Delete(int quizId);
    }
}
