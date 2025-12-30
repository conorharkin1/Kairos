namespace DashboardServer.Tests.Service
{
    using DashboardServer.Exceptions;
    using DashboardServer.Models;
    using DashboardServer.Repositories.Interfaces;
    using DashboardServer.Services;
    using Moq;
    using Xunit;

    public class AssignmentServiceTests
    {
        private readonly Mock<IAssignmentRepository> _mockRepo;
        private readonly AssignmentService _service;

        public AssignmentServiceTests()
        {
            _mockRepo = new Mock<IAssignmentRepository>();
            _service = new AssignmentService(_mockRepo.Object);
        }

        [Fact]
        public async Task Create_ShouldCallRepository_WhenValid()
        {
            // Arrange
            var userId = "user123";
            var assignment = new Assignment
            {
                Name = "Test",
                DueDate = DateTime.UtcNow.AddDays(1),
                Description = "Test Description",
                HtmlUrl = "http://example.com"
            };
            var createdAssignment = new Assignment
            {
                Id = 1,
                Name = "Test",
                DueDate = assignment.DueDate,
                Description = assignment.Description,
                HtmlUrl = assignment.HtmlUrl,
                UserId = userId
            };

            _mockRepo.Setup(r => r.Create(userId, assignment))
                .ReturnsAsync(createdAssignment);

            var result = await _service.Create(userId, assignment);


            Assert.Equal(createdAssignment, result);
            _mockRepo.Verify(r => r.Create(userId, assignment), Times.Once);
        }

        [Fact]
        public async Task Create_ShouldThrowBadRequestException_WhenNameIsEmpty()
        {
            // Arrange
            var userId = "user123";
            var assignment = new Assignment
            {
                Name = "",
                DueDate = DateTime.UtcNow.AddDays(1),
                Description = "Test Description",
                HtmlUrl = "http://example.com"
            };

            // Act & Assert            
            await Assert.ThrowsAsync<BadRequestException>(() => _service.Create(userId, assignment));
            _mockRepo.Verify(r => r.Create(userId, assignment), Times.Never);
        }

        [Fact]
        public async Task Create_ShouldThrowBadRequestException_WhenDueDateIsInPast()
        {
            var userId = "user123";
            // Arrange
            var assignment = new Assignment
            {
                Name = "Test Assignment",
                DueDate = DateTime.UtcNow.AddDays(-1),
                Description = "Test Description",
                HtmlUrl = "http://example.com"
            };

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _service.Create(userId, assignment));
            _mockRepo.Verify(r => r.Create(userId, assignment), Times.Never);
        }

        [Fact]
        public async Task Update_ShouldCallRepository_WhenValid()
        {
            // Arrange
            var userId = "user123";
            var assignment = new Assignment
            {
                Id = 1,
                Name = "Test",
                DueDate = DateTime.UtcNow.AddDays(1),
                UserId = userId
            };
            var updatedAssignment = new Assignment
            {
                Id = 1,
                Name = "Test_Updated",
                DueDate = assignment.DueDate,
                UserId = userId
            };

            _mockRepo.Setup(r => r.Update(userId, assignment.Id, updatedAssignment))
                .ReturnsAsync(updatedAssignment);

            var result = await _service.Update(userId, assignment.Id, updatedAssignment);


            Assert.Equal(updatedAssignment, result);
            _mockRepo.Verify(r => r.Update(userId, assignment.Id, updatedAssignment), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldThrowBadRequestException_WhenNameIsEmpty()
        {
            // Arrange
            var userId = "user123";
            var assignment = new Assignment
            {
                Id = 1,
                Name = "",
                DueDate = DateTime.UtcNow.AddDays(1)
            };

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _service.Update(userId, assignment.Id, assignment));
            _mockRepo.Verify(r => r.Update(userId, assignment.Id, assignment), Times.Never);
        }

        [Fact]
        public async Task Update_ShouldThrowBadRequestException_WhenDueDateIsInPast()
        {
            var userId = "user123";
            // Arrange
            var assignment = new Assignment
            {
                Id = 1,
                Name = "Test Assignment",
                DueDate = DateTime.UtcNow.AddDays(-1)
            };

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _service.Update(userId, assignment.Id, assignment));
            _mockRepo.Verify(r => r.Update(userId, assignment.Id, assignment), Times.Never);
        }
    }
}