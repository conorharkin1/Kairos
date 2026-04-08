using DashboardServer.Data;
using DashboardServer.Models;
using DashboardServer.Repositories;
using DashboardServer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace DashboardServer.Tests.Repository
{
    public class AssignmentRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly AssignmentRepository _repository;

        public AssignmentRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var userResolverMock = new Mock<IUserResolverService>();
            userResolverMock.Setup(x => x.UserId).Returns("user-123");

            _dbContext = new ApplicationDbContext(options, userResolverMock.Object);
            _repository = new AssignmentRepository(_dbContext);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        private Assignment BuildAssignment(string userId = "user-123", int courseId = 1, string name = "Test Assignment", string? description = null, DateTime? dueDate = null, string? htmlUrl = null, bool isComplete = false, bool isPriority = false)
        {
            return new Assignment(userId, courseId, name, description, dueDate ?? DateTime.UtcNow.AddDays(7), htmlUrl, isComplete, isPriority);
        }

        [Fact]
        public async Task CreateAsync_ValidAssignment_ReturnsAssignmentWithGeneratedId()
        {
            var assignment = BuildAssignment();

            var result = await _repository.CreateAsync(assignment);

            Assert.NotNull(result);
            Assert.True(result.Id > 0);
        }

        [Fact]
        public async Task CreateAsync_ValidAssignment_PersistsToDatabase()
        {
            var assignment = BuildAssignment(name: "Persisted Assignment");

            var result = await _repository.CreateAsync(assignment);

            var fromDb = await _dbContext.Assignments.FindAsync(result.Id);
            Assert.NotNull(fromDb);
            Assert.Equal("Persisted Assignment", fromDb.Name);
        }

        [Fact]
        public async Task DeleteAsync_ExistingAssignment_RemovesFromDatabase()
        {
            var assignment = BuildAssignment();
            await _repository.CreateAsync(assignment);

            await _repository.DeleteAsync(assignment);

            var fromDb = await _dbContext.Assignments.FindAsync(assignment.Id);
            Assert.Null(fromDb);
        }
    }
}