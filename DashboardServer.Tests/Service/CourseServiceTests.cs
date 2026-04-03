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

    public class CourseServiceTests
    {
        private readonly Mock<ICourseRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CourseService _service;

        public CourseServiceTests()
        {
            _mockRepo = new Mock<ICourseRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new CourseService(_mockRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidRequest_ReturnsResponseDTO()
        {
            // Arrange
            var userId = "user-123";
            var dto = new CourseRequestDTO
            {
                CourseCode = "CSC3032",
                Name = "Software Engineering",
                ExternalCourseId = 42
            };

            var createdCourse = new Course(userId, dto.CourseCode, dto.Name);
            createdCourse.SetExternalCourseId(dto.ExternalCourseId);

            var expectedResponse = new CourseResponseDTO { Name = dto.Name, CourseCode = dto.CourseCode };

            _mockRepo.Setup(r => r.CreateAsync(It.IsAny<Course>()))
                     .ReturnsAsync(createdCourse);

            _mockMapper.Setup(m => m.Map<CourseResponseDTO>(createdCourse))
                       .Returns(expectedResponse);

            // Act
            var result = await _service.CreateAsync(userId, dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dto.Name, result.Name);
            Assert.Equal(dto.CourseCode, result.CourseCode);
        }

        [Fact]
        public async Task CreateAsync_ValidRequest_CallsRepositoryOnce()
        {
            // Arrange
            var userId = "user-123";
            var dto = new CourseRequestDTO { CourseCode = "CSC3032", Name = "Software Engineering" };

            _mockRepo.Setup(r => r.CreateAsync(It.IsAny<Course>()))
                     .ReturnsAsync(new Course(userId, dto.CourseCode, dto.Name));

            _mockMapper.Setup(m => m.Map<CourseResponseDTO>(It.IsAny<Course>()))
                       .Returns(new CourseResponseDTO());

            // Act
            await _service.CreateAsync(userId, dto);

            // Assert
            _mockRepo.Verify(r => r.CreateAsync(It.IsAny<Course>()), Times.Once);
        }

        [Theory]
        [InlineData("", "CSC3032")]
        [InlineData(" ", "CSC3032")]
        [InlineData(null, "CSC3032")]
        [InlineData("Software Engineering", "")]
        [InlineData("Software Engineering", " ")]
        [InlineData("Software Engineering", null)]
        public async Task CreateAsync_InvalidNameOrCode_ThrowsValidationException(string? name, string? code)
        {
            // Arrange
            var dto = new CourseRequestDTO
            {
                Name = name!,
                CourseCode = code!
            };

            // Act + Assert
            await Assert.ThrowsAsync<ValidationException>(() => _service.CreateAsync("user-123", dto));
        }

        [Fact]
        public async Task CreateAsync_WhenValidationFails_RepositoryIsNeverCalled()
        {
            // Arrange
            var dto = new CourseRequestDTO
            {
                Name = "",
                CourseCode = "CSC3032"
            };

            // Act
            try
            {
                await _service.CreateAsync("user-123", dto);
            }
            catch
            {
                // ignored
            }

            // Assert
            _mockRepo.Verify(r => r.CreateAsync(It.IsAny<Course>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ValidData_UpdatesAndReturnsDTO()
        {
            // Arrange
            var userId = "user-123";
            var courseId = 1;
            var existingCourse = new Course(userId, "CSC3032", "Old Name");
            var updateDto = new CourseRequestDTO { CourseCode = "CSC3033", Name = "New Name" };
            var expectedResponse = new CourseResponseDTO { CourseCode = "CSC3033", Name = "New Name" };

            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                     .ReturnsAsync(existingCourse);

            _mockMapper.Setup(m => m.Map<CourseResponseDTO>(existingCourse))
                       .Returns(expectedResponse);

            // Act
            var result = await _service.UpdateAsync(userId, courseId, updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Name", result.Name);
            Assert.Equal("CSC3033", result.CourseCode);
        }

        [Theory]
        [InlineData("", "CSC3032")]
        [InlineData(" ", "CSC3032")]
        [InlineData(null, "CSC3032")]
        [InlineData("Software Engineering", "")]
        [InlineData("Software Engineering", " ")]
        [InlineData("Software Engineering", null)]
        public async Task UpdateAsync_InvalidNameOrCode_ThrowsValidationException(string? name, string? code)
        {
            // Arrange
            var userId = "user-123";
            var courseId = 1;
            var existingCourse = new Course(userId, "CSC3032", "Old Name");
            var updateDto = new CourseRequestDTO { CourseCode = code!, Name = name! };

            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                     .ReturnsAsync(existingCourse);

            // Act + Assert
            await Assert.ThrowsAsync<ValidationException>(() => _service.UpdateAsync(userId, courseId, updateDto));
        }

        [Fact]
        public async Task SetExternalCourseId_UpdatesExternalCourseId()
        {
            // Arrange
            var userId = "user-123";
            var course = new Course(userId, "CSC3032", "Test Course");
            int externalCourseId = 987;

            // Act
            course.SetExternalCourseId(externalCourseId);

            // Assert
            Assert.Equal(externalCourseId, course.ExternalCourseId);
        }

        [Fact]
        public async Task DeleteAsync_UserNotAuthorized_ThrowsUnAuthorizedException()
        {
            // Arrange
            var userId = "user-123";
            var otherUserId = "user-456";
            var courseId = 1;
            var existingCourse = new Course(otherUserId, "CSC3032", "Test Course");

            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                     .ReturnsAsync(existingCourse);

            // Act + Assert
            await Assert.ThrowsAsync<UnAuthorizedException>(() => _service.DeleteAsync(userId, courseId));
        }

        [Fact]
        public async Task DeleteAsync_NonExistentCourse_ThrowsNotFoundException()
        {
            // Arrange
            var userId = "user-123";
            var courseId = 999;

            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                     .ReturnsAsync((Course?)null);

            // Act + Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteAsync(userId, courseId));
        }

        [Fact]
        public async Task GetCourseAssignmentsAsync_InvalidCourseId_ThrowsBadRequestException()
        {
            // Arrange
            var invalidCourseId = 0;

            // Act + Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _service.GetCourseAssignmentsAsync(invalidCourseId, true));
        }

        [Fact]
        public async Task GetCourseAssignmentsAsync_ValidId_MapsAndReturnsDtos()
        {
            // Arrange
            var courseId = 1;
            var assignments = new List<Assignment>
            {
                new Assignment("user-123", courseId, "A1", null, DateTime.UtcNow.AddDays(1), null, false, false)
            };
            var assignmentDtos = new List<AssignmentResponseDTO>
            {
                new AssignmentResponseDTO { Name = "A1" }
            };

            _mockRepo.Setup(r => r.GetCourseAssignmentsAsync(courseId, true))
                     .ReturnsAsync(assignments);

            _mockMapper.Setup(m => m.Map<List<AssignmentResponseDTO>>(assignments))
                       .Returns(assignmentDtos);

            // Act
            var result = await _service.GetCourseAssignmentsAsync(courseId, true);

            // Assert
            Assert.Single(result);
            Assert.Equal("A1", result[0].Name);
        }

        [Fact]
        public async Task GetCourseQuizzesAsync_InvalidCourseId_ThrowsBadRequestException()
        {
            // Arrange
            var invalidCourseId = 0;

            // Act + Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _service.GetCourseQuizzesAsync(invalidCourseId, true));
        }

        [Fact]
        public async Task GetCourseQuizzesAsync_ValidId_MapsAndReturnsDtos()
        {
            // Arrange
            var courseId = 1;
            var quizzes = new List<Quiz>
            {
                new Quiz("user-123", courseId, "Q1", DateTime.UtcNow.AddDays(1), null, null, false, false)
            };
            var quizDtos = new List<QuizResponseDTO>
            {
                new QuizResponseDTO { Name = "Q1" }
            };

            _mockRepo.Setup(r => r.GetCourseQuizzesAsync(courseId, true))
                     .ReturnsAsync(quizzes);

            _mockMapper.Setup(m => m.Map<List<QuizResponseDTO>>(quizzes))
                       .Returns(quizDtos);

            // Act
            var result = await _service.GetCourseQuizzesAsync(courseId, true);

            // Assert
            Assert.Single(result);
            Assert.Equal("Q1", result[0].Name);
        }
    }
}
