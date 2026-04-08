namespace DashboardServer.Tests.Service
{
    using AutoMapper;
    using DashboardServer.DTOs.ModelDTOs.Requests;
    using DashboardServer.DTOs.ModelDTOs.Responses;
    using DashboardServer.Exceptions;
    using DashboardServer.Models;
    using DashboardServer.Repositories.Interfaces;
    using DashboardServer.Services;
    using Moq;
    using Xunit;

    public class QuizServiceTests
    {
        private readonly Mock<IQuizRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly QuizService _service;

        public QuizServiceTests()
        {
            _mockRepo = new Mock<IQuizRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new QuizService(_mockRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidRequest_ReturnsResponseDTO()
        {
            // Arrange
            var userId = "user-123";
            var dto = new QuizRequestDTO
            {
                CourseId = 1,
                Name = "Week 1 Quiz",
                DueDate = DateTime.UtcNow.AddDays(7),
                LockDate = DateTime.UtcNow.AddDays(8),
                HtmlUrl = null,
                IsComplete = false,
                IsPriority = true
            };

            var createdQuiz = new Quiz(userId, dto.CourseId, dto.Name,
                dto.DueDate, dto.LockDate, dto.HtmlUrl, dto.IsComplete, dto.IsPriority);

            var expectedResponse = new QuizResponseDTO { Name = dto.Name };

            _mockRepo.Setup(r => r.CreateAsync(It.IsAny<Quiz>()))
                     .ReturnsAsync(createdQuiz);

            _mockMapper.Setup(m => m.Map<QuizResponseDTO>(createdQuiz))
                       .Returns(expectedResponse);

            // Act
            var result = await _service.CreateAsync(userId, dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dto.Name, result.Name);
        }

        [Fact]
        public async Task CreateAsync_ValidRequest_CallsRepositoryOnce()
        {
            // Arrange
            var userId = "user-123";
            var dto = new QuizRequestDTO { CourseId = 1, Name = "Week 1 Quiz" };

            _mockRepo.Setup(r => r.CreateAsync(It.IsAny<Quiz>()))
                     .ReturnsAsync(new Quiz(userId, 1, "Week 1 Quiz",
                         null, null, null, false, false));

            _mockMapper.Setup(m => m.Map<QuizResponseDTO>(It.IsAny<Quiz>()))
                       .Returns(new QuizResponseDTO());

            // Act
            await _service.CreateAsync(userId, dto);

            // Assert
            _mockRepo.Verify(r => r.CreateAsync(It.IsAny<Quiz>()), Times.Once);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task CreateAsync_InvalidName_ThrowsValidationException(string? invalidName)
        {
            // Arrange
            var dto = new QuizRequestDTO { CourseId = 1, Name = invalidName! };

            // Act + Assert
            await Assert.ThrowsAsync<ValidationException>(() => _service.CreateAsync("user-123", dto));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-999)]
        public async Task CreateAsync_InvalidCourseId_ThrowsValidationException(int invalidCourseId)
        {
            // Arrange
            var dto = new QuizRequestDTO { CourseId = invalidCourseId, Name = "Valid Name" };

            // Act + Assert
            await Assert.ThrowsAsync<ValidationException>(() =>
                _service.CreateAsync("user-123", dto));
        }

        [Fact]
        public async Task CreateAsync_WhenValidationFails_RepositoryIsNeverCalled()
        {
            // Arrange — ensures we don't hit the DB on bad input
            var dto = new QuizRequestDTO { CourseId = 1, Name = "" };

            // Act
            try
            {
                await _service.CreateAsync("user-123", dto);
            }
            catch
            {
                // Ignore exception
            }

            // Assert
            _mockRepo.Verify(r => r.CreateAsync(It.IsAny<Quiz>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ValidData_UpdatesAndReturnsDTO()
        {
            // Arrange
            var userId = "user-123";
            var quizId = 1;
            var existingQuiz = new Quiz(userId, 1, "Old Name", null, null, null, false, false);
            var updateDto = new QuizRequestDTO { CourseId = 1, Name = "New Name" };
            var expectedResponse = new QuizResponseDTO { Name = "New Name" };

            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                     .ReturnsAsync(existingQuiz);

            _mockMapper.Setup(m => m.Map<QuizResponseDTO>(existingQuiz))
                       .Returns(expectedResponse);

            // Act
            var result = await _service.UpdateAsync(userId, quizId, updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Name", result.Name);
        }

        [Fact]
        public async Task UpdateAsync_InvalidName_ThrowsValidationException()
        {
            // Arrange
            var userId = "user-123";
            var quizId = 1;
            var existingQuiz = new Quiz(userId, 1, "Old Name", null, null, null, false, false);
            var updateDto = new QuizRequestDTO { CourseId = 1, Name = "" };

            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                     .ReturnsAsync(existingQuiz);

            // Act + Assert
            await Assert.ThrowsAsync<ValidationException>(() => _service.UpdateAsync(userId, quizId, updateDto));
        }

        [Fact]
        public async Task SetDueDate_UpdatesDueDate()
        {
            // Arrange
            var userId = "user-123";
            var quiz = new Quiz(userId, 1, "Test Quiz", DateTime.UtcNow.AddDays(7), null, null, false, false);
            var newDueDate = DateTime.UtcNow.AddDays(14);

            // Act
            quiz.SetDueDate(newDueDate);

            // Assert
            Assert.Equal(newDueDate, quiz.DueDate);
        }

        [Fact]
        public async Task SetLockDate_UpdatesLockDate()
        {
            // Arrange
            var userId = "user-123";
            var quiz = new Quiz(userId, 1, "Test Quiz", DateTime.UtcNow.AddDays(7), null, null, false, false);
            var newLockDate = DateTime.UtcNow.AddDays(10);

            // Act
            quiz.SetLockDate(newLockDate);

            // Assert
            Assert.Equal(newLockDate, quiz.LockDate);
        }

        [Fact]
        public async Task SetExternalQuizId_UpdatesExternalQuizId()
        {
            // Arrange
            var userId = "user-123";
            var quiz = new Quiz(userId, 1, "Test Quiz", DateTime.UtcNow.AddDays(7), null, null, false, false);
            int externalQuizId = 987;

            // Act
            quiz.SetExternalQuizId(externalQuizId);

            // Assert
            Assert.Equal(externalQuizId, quiz.ExternalQuizId);
        }

        [Fact]
        public async Task DeleteAsync_UserNotAuthorized_ThrowsUnAuthorizedException()
        {
            // Arrange
            var userId = "user-123";
            var otherUserId = "user-456";
            var quizId = 1;
            var existingQuiz = new Quiz(otherUserId, 1, "Test Quiz", null, null, null, false, false);

            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                     .ReturnsAsync(existingQuiz);

            // Act + Assert
            await Assert.ThrowsAsync<UnAuthorizedException>(() => _service.DeleteAsync(userId, quizId));
        }

        [Fact]
        public async Task DeleteAsync_NonExistentQuiz_ThrowsNotFoundException()
        {
            // Arrange
            var userId = "user-123";
            var quizId = 999;

            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                     .ReturnsAsync((Quiz?)null);

            // Act + Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteAsync(userId, quizId));
        }
    }
}
