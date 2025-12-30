using DashboardServer.Exceptions;
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
            if (string.IsNullOrWhiteSpace(quiz.Name))
            {
                throw new BadRequestException("Quiz name cannot be empty");
            }

            if (quiz.DueDate != null && quiz.DueDate < DateTime.UtcNow)
            {
                throw new BadRequestException("Due date cannot be in the past");
            }

            if (quiz.LockDate != null && quiz.LockDate < DateTime.UtcNow)
            {
                throw new BadRequestException("Lock date cannot be in the past");
            }

            if (quiz.DueDate != null && quiz.LockDate != null && quiz.LockDate < quiz.DueDate)
            {
                throw new BadRequestException("Lock date cannot be before due date");
            }

            return await _quizRepository.Create(userId, quiz);
        }

        public async Task<Quiz> Update(string userId, int quizId, Quiz quiz)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedException("User is not authorized");
            }

            if (string.IsNullOrWhiteSpace(quiz.Name))
            {
                throw new BadRequestException("Quiz name cannot be empty");
            }

            if (quiz.DueDate != null && quiz.DueDate < DateTime.UtcNow)
            {
                throw new BadRequestException("Due date cannot be in the past");
            }

            if (quiz.LockDate != null && quiz.LockDate < DateTime.UtcNow)
            {
                throw new BadRequestException("Lock date cannot be in the past");
            }

            if (quiz.DueDate != null && quiz.LockDate != null && quiz.LockDate < quiz.DueDate)
            {
                throw new BadRequestException("Lock date cannot be before due date");
            }

            return await _quizRepository.Update(userId, quizId, quiz);
        }

        public async Task Delete(string userId, int quizId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedException("User is not authorized");
            }
            
            await _quizRepository.Delete(userId, quizId);
        }
    }
}
