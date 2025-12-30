using DashboardServer.Models;

namespace DashboardServer.Services.Interfaces
{
    public interface IQuizService
    {
        public Task<Quiz> Create(string userId, Quiz quiz);
        public Task<Quiz> Update(string userId, int quizId, Quiz quiz);
        public Task Delete(string userId, int quizId);
    }
}
