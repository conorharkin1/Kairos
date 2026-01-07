using DashboardServer.Data;
using DashboardServer.Models;
using DashboardServer.Repositories;
using DashboardServer.Repositories.Interfaces;
using DashboardServer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace DashboardServer.Tests.Repository
{
    public class AssignmentRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly AssignmentRepository _repository;
        private readonly IUserResolverService _userResolverService;

        public AssignmentRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var userResolverMock = new Mock<IUserResolverService>();
            userResolverMock.Setup(x => x.UserId).Returns("123");

            _userResolverService = userResolverMock.Object;
            _dbContext = new ApplicationDbContext(options, _userResolverService);
            _repository = new AssignmentRepository(_dbContext);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        #region Create Tests

        [Fact]
        public async Task Create_ValidAssignment_ReturnsCreatedAssignmentWithIdAndUserId()
        {
            // Arrange
            var userId = "123";
            var assignment = new Assignment
            {
                Name = "Test Assignment",
                DueDate = DateTime.Now.AddDays(7),
            };

            // Act
            var created = await _repository.Create(userId, assignment);

            // Assert
            Assert.NotNull(created);
            Assert.True(created.Id > 0);
            Assert.Equal(userId, created.UserId);
            Assert.Equal("Test Assignment", created.Name);
        }

        #endregion

        #region Update Tests

        [Fact]
        public async Task Update_ExistingAssignment_UpdatesAndReturnsAssignment()
        {
            var userId = "123";

            var assignment = new Assignment
            {
                Name = "Assignment 1",
                DueDate = DateTime.Now.AddDays(1),
                UserId = userId
            };

            _dbContext.Assignments.Add(assignment);
            await _dbContext.SaveChangesAsync();

            var updatedAssignment = new Assignment
            {
                Name = "Updated Assignment 1",
                DueDate = assignment.DueDate
            };

            var updated = await _repository.Update(userId, assignment.Id, updatedAssignment);

            Assert.NotNull(updated);
            Assert.Equal("Updated Assignment 1", updated.Name);

            var fromDb = await _dbContext.Assignments.FindAsync(assignment.Id);
            Assert.NotNull(fromDb);
            Assert.Equal("Updated Assignment 1", fromDb.Name);
        }

        [Fact]
        public async Task Update_NonExistentAssignment_ThrowsKeyNotFoundException()
        {
            // Arrange
            var userId = "123";
            var nonExistentId = 999;
            var updateData = new Assignment { Name = "Test" };

            // Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _repository.Update(userId, nonExistentId, updateData));

            Assert.Equal("Assignment not found", exception.Message);
        }

        [Fact]
        public async Task Update_WithPartialData_UpdatesOnlyProvidedFields()
        {
            // Arrange
            var userId = "123";
            var assignment = new Assignment
            {
                Name = "Original",
                Description = "Original Description",
                DueDate = DateTime.Now,
                HtmlUrl = "https://original.com"
            };
            var created = await _repository.Create(userId, assignment);

            var updateData = new Assignment
            {
                Name = "Updated Name Only",
                Description = created.Description,
                DueDate = created.DueDate,
                HtmlUrl = created.HtmlUrl
            };

            // Act
            var result = await _repository.Update(userId, created.Id, updateData);

            // Assert
            Assert.Equal("Updated Name Only", result.Name);
            Assert.Equal("Original Description", result.Description);
        }

        #endregion

        #region Delete Tests

        [Fact]
        public async Task Delete_ExistingAssignment_RemovesAssignment()
        {
            // Arrange
            var userId = "123";
            var assignment = new Assignment
            {
                Name = "To Be Deleted",
                DueDate = DateTime.Now.AddDays(3),
                UserId = userId
            };
            _dbContext.Assignments.Add(assignment);
            await _dbContext.SaveChangesAsync();

            // Act
            await _repository.Delete(userId, assignment.Id);

            // Assert
            var deleted = await _dbContext.Assignments.Where(a => a.Id == assignment.Id).FirstOrDefaultAsync();
            Assert.Null(deleted);
        }

        [Fact]
        public async Task Delete_NonExistentAssignment_ThrowsKeyNotFoundException()
        {
            // Arrange
            var userId = "123";
            var nonExistentId = 999;

            // Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _repository.Delete(userId, nonExistentId));

            Assert.Equal("Assignment not found", exception.Message);
        }

        #endregion
    }
}