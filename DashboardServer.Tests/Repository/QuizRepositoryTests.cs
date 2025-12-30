using DashboardServer.Data;
using DashboardServer.Models;
using DashboardServer.Repositories;
using DashboardServer.Repositories.Interfaces;
using DashboardServer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace DashboardServer.Tests.Repository
{
    public class QuizRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly QuizRepository _repository;
        private readonly IUserResolverService _userResolverService;

        public QuizRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var userResolverMock = new Mock<IUserResolverService>();
            userResolverMock.Setup(x => x.UserId).Returns("123");

            _userResolverService = userResolverMock.Object;
            _dbContext = new ApplicationDbContext(options, _userResolverService);
            _repository = new QuizRepository(_dbContext);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        #region Create Tests

        [Fact]
        public async Task Create_ValidQuiz_ReturnsCreatedQuizWithIdAndUserId()
        {
            // Arrange
            var userId = "123";
            var quiz = new Quiz
            {
                Name = "Test Quiz",
                DueDate = DateTime.Now.AddDays(7),
                LockDate = DateTime.Now.AddDays(8),
                CourseId = 1
            };

            // Act
            var created = await _repository.Create(userId, quiz);

            // Assert
            Assert.NotNull(created);
            Assert.True(created.Id > 0);
            Assert.Equal(userId, created.UserId);
            Assert.Equal("Test Quiz", created.Name);
        }

        #endregion

        #region Update Tests

        [Fact]
        public async Task Update_ExistingQuiz_UpdatesAndReturnsQuiz()
        {
            var userId = "123";

            var quiz = new Quiz
            {
                Name = "Quiz 1",
                DueDate = DateTime.Now.AddDays(1),
                LockDate = DateTime.Now.AddDays(2),
                CourseId = 1,
                UserId = userId
            };

            _dbContext.Quizzes.Add(quiz);
            await _dbContext.SaveChangesAsync();

            var updatedQuiz = new Quiz
            {
                Name = "Updated Quiz 1",
                DueDate = quiz.DueDate,
                LockDate = quiz.LockDate
            };

            var updated = await _repository.Update(userId, quiz.Id, updatedQuiz);

            Assert.NotNull(updated);
            Assert.Equal("Updated Quiz 1", updated.Name);

            var fromDb = await _dbContext.Quizzes.FindAsync(quiz.Id);
            Assert.NotNull(fromDb);
            Assert.Equal("Updated Quiz 1", fromDb.Name);
        }

        [Fact]
        public async Task Update_NonExistentQuiz_ThrowsKeyNotFoundException()
        {
            // Arrange
            var userId = "123";
            var nonExistentId = 999;
            var updateData = new Quiz { Name = "Test" };

            // Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _repository.Update(userId, nonExistentId, updateData));

            Assert.Equal("Quiz not found", exception.Message);
        }

        [Fact]
        public async Task Update_WithPartialData_UpdatesOnlyProvidedFields()
        {
            // Arrange
            var userId = "123";
            var quiz = new Quiz
            {
                Name = "Original",
                DueDate = DateTime.Now,
                LockDate = DateTime.Now.AddDays(1),
                HtmlUrl = "https://original.com",
                CourseId = 1
            };
            var created = await _repository.Create(userId, quiz);

            var updateData = new Quiz
            {
                Name = "Updated Name Only",
                DueDate = created.DueDate,
                LockDate = created.LockDate,
                HtmlUrl = created.HtmlUrl
            };

            // Act
            var result = await _repository.Update(userId, created.Id, updateData);

            // Assert
            Assert.Equal("Updated Name Only", result.Name);
            Assert.Equal("https://original.com", result.HtmlUrl);
        }

        #endregion

        #region Delete Tests

        [Fact]
        public async Task Delete_ExistingQuiz_RemovesQuiz()
        {
            // Arrange
            var userId = "123";
            var quiz = new Quiz
            {
                Name = "To Be Deleted",
                DueDate = DateTime.Now.AddDays(3),
                CourseId = 1,
                UserId = userId
            };
            _dbContext.Quizzes.Add(quiz);
            await _dbContext.SaveChangesAsync();

            // Act
            await _repository.Delete(userId, quiz.Id);

            // Assert
            var deleted = await _dbContext.Quizzes.Where(q => q.Id == quiz.Id).FirstOrDefaultAsync();
            Assert.Null(deleted);
        }

        [Fact]
        public async Task Delete_NonExistentQuiz_ThrowsKeyNotFoundException()
        {
            // Arrange
            var userId = "123";
            var nonExistentId = 999;

            // Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _repository.Delete(userId, nonExistentId));

            Assert.Equal("Quiz not found", exception.Message);
        }

        #endregion
    }
}
