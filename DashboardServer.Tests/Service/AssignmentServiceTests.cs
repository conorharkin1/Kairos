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
    using Xunit.Sdk;

    public class AssignmentServiceTests
    {
        private readonly Mock<IAssignmentRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly AssignmentService _service;

        public AssignmentServiceTests()
        {
            _mockRepo = new Mock<IAssignmentRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new AssignmentService(_mockRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidRequest_ReturnsResponseDTO()
        {
            // Arrange
            var userId = "user-123";
            var dto = new AssignmentRequestDTO
            {
                CourseId = 1,
                Name = "Week 1 Essay",
                Description = "Write 500 words",
                DueDate = DateTime.UtcNow.AddDays(7),
                HtmlUrl = null,
                IsComplete = false,
                IsPriority = true
            };

            var createdAssignment = new Assignment(userId, dto.CourseId, dto.Name,
                dto.Description, dto.DueDate, dto.HtmlUrl, dto.IsComplete, dto.IsPriority);

            var expectedResponse = new AssignmentResponseDTO { Name = dto.Name };

            _mockRepo.Setup(r => r.CreateAsync(It.IsAny<Assignment>()))
                    .ReturnsAsync(createdAssignment);

            _mockMapper.Setup(m => m.Map<AssignmentResponseDTO>(createdAssignment))
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
            var dto = new AssignmentRequestDTO { CourseId = 1, Name = "Week 1 Essay" };

            _mockRepo.Setup(r => r.CreateAsync(It.IsAny<Assignment>()))
                    .ReturnsAsync(new Assignment(userId, 1, "Week 1 Essay",
                        null, null, null, false, false));

            _mockMapper.Setup(m => m.Map<AssignmentResponseDTO>(It.IsAny<Assignment>()))
                    .Returns(new AssignmentResponseDTO());

            // Act
            await _service.CreateAsync(userId, dto);

            // Assert
            _mockRepo.Verify(r => r.CreateAsync(It.IsAny<Assignment>()), Times.Once);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task CreateAsync_InvalidName_ThrowsValidationException(string invalidName)
        {
            // Arrange
            var dto = new AssignmentRequestDTO { CourseId = 1, Name = invalidName };

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
            var dto = new AssignmentRequestDTO { CourseId = invalidCourseId, Name = "Valid Name" };

            // Act + Assert
            await Assert.ThrowsAsync<ValidationException>(() =>
                _service.CreateAsync("user-123", dto));
        }

        [Fact]
        public async Task CreateAsync_WhenValidationFails_RepositoryIsNeverCalled()
        {
            // Arrange — ensures we don't hit the DB on bad input
            var dto = new AssignmentRequestDTO { CourseId = 1, Name = "" };

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
            _mockRepo.Verify(r => r.CreateAsync(It.IsAny<Assignment>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ValidData_UpdatesAndReturnsDTO()
        {
            // Arrange
            var userId = "user-123";
            var assignmentId = 1;
            var existingAssignment = new Assignment(userId, 1, "Old Name", null, null, null, false, false);
            var updateDto = new AssignmentRequestDTO { CourseId = 1, Name = "New Name" };
            var expectedResponse = new AssignmentResponseDTO { Name = "New Name" };

            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync(existingAssignment);

            _mockMapper.Setup(m => m.Map<AssignmentResponseDTO>(existingAssignment))
                    .Returns(expectedResponse);

            // Act
            var result = await _service.UpdateAsync(userId, assignmentId, updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Name", result.Name);
        }

        [Fact]
        public async Task UpdateAsync_InvalidName_ThrowsValidationException()
        {
            // Arrange
            var userId = "user-123";
            var assignmentId = 1;
            var existingAssignment = new Assignment(userId, 1, "Old Name", null, null, null, false, false);
            var updateDto = new AssignmentRequestDTO { CourseId = 1, Name = "" };

            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync(existingAssignment);

            // Act + Assert
            await Assert.ThrowsAsync<ValidationException>(() => _service.UpdateAsync(userId, assignmentId, updateDto));
        }

        [Fact]
        public async Task SetDueDate_UpdatesDueDate()
        {
            // Arrange
            var userId = "user-123";
            var assignment = new Assignment(userId, 1, "Test Assignment", null, DateTime.UtcNow.AddDays(7), null, false, false);
            var newDueDate = DateTime.UtcNow.AddDays(14);

            // Act
            assignment.SetDueDate(newDueDate);

            // Assert
            Assert.Equal(newDueDate, assignment.DueDate);
        }

        [Fact]
        public async Task SetExternalAssignmentId_UpdatesExternalAssignmentId()
        {
            // Arrange
            var userId = "user-123";
            var assignment = new Assignment(userId, 1, "Test Assignment", null, DateTime.UtcNow.AddDays(7), null, false, false);
            int externalAssignmentId = 987;

            // Act
            assignment.SetExternalAssignmentId(externalAssignmentId);

            // Assert
            Assert.Equal(externalAssignmentId, assignment.ExternalAssignmentId);
        }

        [Fact]
        public async Task DeleteAsync_UserNotAuthorized_ThrowsUnAuthorizedException()
        {
            // Arrange
            var userId = "user-123";
            var otherUserId = "user-456";
            var assignmentId = 1;
            var existingAssignment = new Assignment(otherUserId, 1, "Test Assignment", null, null, null, false, false);

            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync(existingAssignment);

            // Act + Assert
            await Assert.ThrowsAsync<UnAuthorizedException>(() => _service.DeleteAsync(userId, assignmentId));
        }

        [Fact]
        public async Task DeleteAsync_NonExistentAssignment_ThrowsNotFoundException()
        {
            // Arrange
            var userId = "user-123";
            var assignmentId = 999;

            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync((Assignment?)null);

            // Act + Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteAsync(userId, assignmentId));
        }
    }
}