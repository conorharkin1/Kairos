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

        public async Task<List<Quiz>> GetAllAsync()
        {
            return await _dbContext.Quizzes.AsNoTracking().ToListAsync();
        }

        public async Task<Quiz?> GetByIdAsync(int id)
        {
            return await _dbContext.Quizzes.FindAsync(id);
        }

        public async Task<Quiz> CreateAsync(Quiz quiz)
        {
            var createdQuiz = await _dbContext.Quizzes.AddAsync(quiz);
            await _dbContext.SaveChangesAsync();
            return createdQuiz.Entity;
        }

        public async Task CreateBulkAsync(List<Quiz> quizzes)
        {
            await _dbContext.Quizzes.AddRangeAsync(quizzes);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Quiz quiz)
        {
            _dbContext.Quizzes.Remove(quiz);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
