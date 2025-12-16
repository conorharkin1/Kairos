using DashboardServer.Models;
using DashboardServer.Repositories.Interfaces;
using DashboardServer.Services.Interfaces;

namespace DashboardServer.Services
{
    public class QuizService : IQuizService
    {
        private readonly IQuizRepository _quizRepository;
        public QuizService(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public async Task<Quiz> Create(string userId, Quiz quiz)
        {
            return await _quizRepository.Create(userId, quiz);
        }

        public async Task<Quiz> Update(string userId, int quizId, Quiz quiz)
        {
            return await _quizRepository.Update(userId, quizId, quiz);
        }

        public async Task Delete(int quizId)
        {
            await _quizRepository.Delete(quizId);
        }
    }
}
