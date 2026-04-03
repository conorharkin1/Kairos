using DashboardServer.Data;
using DashboardServer.Models;
using DashboardServer.Repositories;
using DashboardServer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace DashboardServer.Tests.Repository
{
    public class QuizRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly QuizRepository _repository;

        public QuizRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var userResolverMock = new Mock<IUserResolverService>();
            userResolverMock.Setup(x => x.UserId).Returns("user-123");

            _dbContext = new ApplicationDbContext(options, userResolverMock.Object);
            _repository = new QuizRepository(_dbContext);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        private Quiz BuildQuiz(string userId = "user-123", int courseId = 1, string name = "Test Quiz",
            DateTime? dueDate = null, DateTime? lockDate = null, string? htmlUrl = null,
            bool isComplete = false, bool isPriority = false)
        {
            return new Quiz(userId, courseId, name,
                dueDate ?? DateTime.UtcNow.AddDays(7), lockDate, htmlUrl, isComplete, isPriority);
        }

        [Fact]
        public async Task CreateAsync_ValidQuiz_ReturnsQuizWithGeneratedId()
        {
            var quiz = BuildQuiz();

            var result = await _repository.CreateAsync(quiz);

            Assert.NotNull(result);
            Assert.True(result.Id > 0);
        }

        [Fact]
        public async Task CreateAsync_ValidQuiz_PersistsToDatabase()
        {
            var quiz = BuildQuiz(name: "Persisted Quiz");

            var result = await _repository.CreateAsync(quiz);

            var fromDb = await _dbContext.Quizzes.FindAsync(result.Id);
            Assert.NotNull(fromDb);
            Assert.Equal("Persisted Quiz", fromDb!.Name);
        }

        [Fact]
        public async Task DeleteAsync_ExistingQuiz_RemovesFromDatabase()
        {
            var quiz = BuildQuiz();
            await _repository.CreateAsync(quiz);

            await _repository.DeleteAsync(quiz);

            var fromDb = await _dbContext.Quizzes.FindAsync(quiz.Id);
            Assert.Null(fromDb);
        }
    }
}
