namespace DashboardServer.Tests.Service
{
    using DashboardServer.Exceptions;
    using DashboardServer.Models;
    using DashboardServer.Repositories.Interfaces;
    using DashboardServer.Services;
    using Moq;
    using Xunit;

    public class QuizServiceTests
    {
        private readonly Mock<IQuizRepository> _mockRepo;
        private readonly QuizService _service;

        public QuizServiceTests()
        {
            _mockRepo = new Mock<IQuizRepository>();
            _service = new QuizService(_mockRepo.Object);
        }

        [Fact]
        public async Task Create_ShouldCallRepository_WhenValid()
        {
            // Arrange
            var userId = "user123";
            var quiz = new Quiz
            {
                Name = "Test Quiz",
                DueDate = DateTime.UtcNow.AddDays(1),
                LockDate = DateTime.UtcNow.AddDays(2),
                HtmlUrl = "http://example.com",
                CourseId = 1
            };
            var createdQuiz = new Quiz
            {
                Id = 1,
                Name = "Test Quiz",
                DueDate = quiz.DueDate,
                LockDate = quiz.LockDate,
                HtmlUrl = quiz.HtmlUrl,
                CourseId = 1,
                UserId = userId
            };

            _mockRepo.Setup(r => r.Create(userId, quiz))
                .ReturnsAsync(createdQuiz);

            var result = await _service.Create(userId, quiz);

            Assert.Equal(createdQuiz, result);
            _mockRepo.Verify(r => r.Create(userId, quiz), Times.Once);
        }

        [Fact]
        public async Task Create_ShouldThrowBadRequestException_WhenNameIsEmpty()
        {
            // Arrange
            var userId = "user123";
            var quiz = new Quiz
            {
                Name = "",
                DueDate = DateTime.UtcNow.AddDays(1),
                LockDate = DateTime.UtcNow.AddDays(2),
                HtmlUrl = "http://example.com",
                CourseId = 1
            };

            // Act & Assert            
            await Assert.ThrowsAsync<BadRequestException>(() => _service.Create(userId, quiz));
            _mockRepo.Verify(r => r.Create(userId, quiz), Times.Never);
        }

        [Fact]
        public async Task Create_ShouldThrowBadRequestException_WhenDueDateIsInPast()
        {
            var userId = "user123";
            // Arrange
            var quiz = new Quiz
            {
                Name = "Test Quiz",
                DueDate = DateTime.UtcNow.AddDays(-1),
                LockDate = DateTime.UtcNow.AddDays(1),
                HtmlUrl = "http://example.com",
                CourseId = 1
            };

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _service.Create(userId, quiz));
            _mockRepo.Verify(r => r.Create(userId, quiz), Times.Never);
        }

        [Fact]
        public async Task Create_ShouldThrowBadRequestException_WhenLockDateIsInPast()
        {
            var userId = "user123";
            // Arrange
            var quiz = new Quiz
            {
                Name = "Test Quiz",
                DueDate = DateTime.UtcNow.AddDays(1),
                LockDate = DateTime.UtcNow.AddDays(-1),
                HtmlUrl = "http://example.com",
                CourseId = 1
            };

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _service.Create(userId, quiz));
            _mockRepo.Verify(r => r.Create(userId, quiz), Times.Never);
        }

        [Fact]
        public async Task Create_ShouldThrowBadRequestException_WhenLockDateBeforeDueDate()
        {
            var userId = "user123";
            // Arrange
            var quiz = new Quiz
            {
                Name = "Test Quiz",
                DueDate = DateTime.UtcNow.AddDays(5),
                LockDate = DateTime.UtcNow.AddDays(3),
                HtmlUrl = "http://example.com",
                CourseId = 1
            };

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _service.Create(userId, quiz));
            _mockRepo.Verify(r => r.Create(userId, quiz), Times.Never);
        }

        [Fact]
        public async Task Update_ShouldCallRepository_WhenValid()
        {
            // Arrange
            var userId = "user123";
            var quiz = new Quiz
            {
                Id = 1,
                Name = "Test Quiz",
                DueDate = DateTime.UtcNow.AddDays(1),
                LockDate = DateTime.UtcNow.AddDays(2),
                UserId = userId,
                CourseId = 1
            };
            var updatedQuiz = new Quiz
            {
                Id = 1,
                Name = "Test Quiz Updated",
                DueDate = quiz.DueDate,
                LockDate = quiz.LockDate,
                UserId = userId,
                CourseId = 1
            };

            _mockRepo.Setup(r => r.Update(userId, quiz.Id, updatedQuiz))
                .ReturnsAsync(updatedQuiz);

            var result = await _service.Update(userId, quiz.Id, updatedQuiz);

            Assert.Equal(updatedQuiz, result);
            _mockRepo.Verify(r => r.Update(userId, quiz.Id, updatedQuiz), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldThrowBadRequestException_WhenNameIsEmpty()
        {
            // Arrange
            var userId = "user123";
            var quiz = new Quiz
            {
                Id = 1,
                Name = "",
                DueDate = DateTime.UtcNow.AddDays(1),
                LockDate = DateTime.UtcNow.AddDays(2),
                CourseId = 1
            };

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _service.Update(userId, quiz.Id, quiz));
            _mockRepo.Verify(r => r.Update(userId, quiz.Id, quiz), Times.Never);
        }

        [Fact]
        public async Task Update_ShouldThrowBadRequestException_WhenDueDateIsInPast()
        {
            var userId = "user123";
            // Arrange
            var quiz = new Quiz
            {
                Id = 1,
                Name = "Test Quiz",
                DueDate = DateTime.UtcNow.AddDays(-1),
                LockDate = DateTime.UtcNow.AddDays(1),
                CourseId = 1
            };

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _service.Update(userId, quiz.Id, quiz));
            _mockRepo.Verify(r => r.Update(userId, quiz.Id, quiz), Times.Never);
        }

        [Fact]
        public async Task Update_ShouldThrowBadRequestException_WhenLockDateIsInPast()
        {
            var userId = "user123";
            // Arrange
            var quiz = new Quiz
            {
                Id = 1,
                Name = "Test Quiz",
                DueDate = DateTime.UtcNow.AddDays(1),
                LockDate = DateTime.UtcNow.AddDays(-1),
                CourseId = 1
            };

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _service.Update(userId, quiz.Id, quiz));
            _mockRepo.Verify(r => r.Update(userId, quiz.Id, quiz), Times.Never);
        }

        [Fact]
        public async Task Update_ShouldThrowBadRequestException_WhenLockDateBeforeDueDate()
        {
            var userId = "user123";
            // Arrange
            var quiz = new Quiz
            {
                Id = 1,
                Name = "Test Quiz",
                DueDate = DateTime.UtcNow.AddDays(5),
                LockDate = DateTime.UtcNow.AddDays(3),
                CourseId = 1
            };

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _service.Update(userId, quiz.Id, quiz));
            _mockRepo.Verify(r => r.Update(userId, quiz.Id, quiz), Times.Never);
        }

        [Fact]
        public async Task Update_ShouldThrowUnauthorizedException_WhenUserIdIsEmpty()
        {
            // Arrange
            var userId = "";
            var quiz = new Quiz
            {
                Id = 1,
                Name = "Test Quiz",
                DueDate = DateTime.UtcNow.AddDays(1),
                CourseId = 1
            };

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedException>(() => _service.Update(userId, quiz.Id, quiz));
            _mockRepo.Verify(r => r.Update(userId, quiz.Id, quiz), Times.Never);
        }

        [Fact]
        public async Task Delete_ShouldCallRepository_WhenValid()
        {
            // Arrange
            var userId = "user123";
            var quizId = 1;

            _mockRepo.Setup(r => r.Delete(userId, quizId))
                .Returns(Task.CompletedTask);

            // Act
            await _service.Delete(userId, quizId);

            // Assert
            _mockRepo.Verify(r => r.Delete(userId, quizId), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldThrowUnauthorizedException_WhenUserIdIsEmpty()
        {
            // Arrange
            var userId = "";
            var quizId = 1;

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedException>(() => _service.Delete(userId, quizId));
            _mockRepo.Verify(r => r.Delete(userId, quizId), Times.Never);
        }
    }
}
