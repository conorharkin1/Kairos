using DashboardServer.Data;
using DashboardServer.Models;
using DashboardServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DashboardServer.Repositories
{
    public class QuizRepository : IQuizRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public QuizRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Quiz> Create(string userId, Quiz quiz)
        {
            quiz.UserId = userId;
            var createdQuiz = await _dbContext.Quizzes.AddAsync(quiz);
            await _dbContext.SaveChangesAsync();
            return createdQuiz.Entity;
        }

        public async Task<Quiz> Update(string userId, int quizId, Quiz quiz)
        {
            var existingQuiz = await _dbContext.Quizzes.FirstOrDefaultAsync(q => q.Id == quizId && q.UserId == userId);
            if (existingQuiz == null)
            {
                throw new KeyNotFoundException("Quiz not found");
            }

            existingQuiz.Name = quiz.Name;
            existingQuiz.DueDate = quiz.DueDate;
            existingQuiz.LockDate = quiz.LockDate;
            existingQuiz.HtmlUrl = quiz.HtmlUrl;
            existingQuiz.IsComplete = quiz.IsComplete;
            existingQuiz.IsFavourite = quiz.IsFavourite;

            await _dbContext.SaveChangesAsync();
            return existingQuiz;
        }

        public async Task Delete(string userId, int quizId)
        {
            var quiz = await _dbContext.Quizzes.FindAsync(quizId);
            if (quiz != null && quiz.UserId == userId)
            {
                _dbContext.Quizzes.Remove(quiz);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Quiz not found");
            }
        }
    }
}
