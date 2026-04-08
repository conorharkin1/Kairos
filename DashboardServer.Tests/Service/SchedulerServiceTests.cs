namespace DashboardServer.Tests.Service
{
    using AutoMapper;
    using DashboardServer.DTOs.ModelDTOs;
    using DashboardServer.Exceptions;
    using DashboardServer.Models;
    using DashboardServer.Models.Constraints;
    using DashboardServer.Repositories.Interfaces;
    using DashboardServer.Services;
    using DashboardServer.Services.Interfaces;
    using Moq;
    using Xunit;

    public class SchedulerServiceTests
    {
        private readonly Mock<IUserResolverService> _userResolverMock;
        private readonly Mock<ISchedulerRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly SchedulerService _service;

        public SchedulerServiceTests()
        {
            _userResolverMock = new Mock<IUserResolverService>();
            _repoMock = new Mock<ISchedulerRepository>();
            _mapperMock = new Mock<IMapper>();

            _userResolverMock.Setup(x => x.UserId).Returns("user-123");

            _service = new SchedulerService(_userResolverMock.Object, _repoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task RunScheduler_ValidData_CallsSaveSchedule()
        {
            // Arrange
            var data = GetSchedulerData();

            _repoMock.Setup(r => r.GetDataForScheduler())
                     .ReturnsAsync(data);

            // Act
            await _service.RunScheduler();

            // Assert
            _repoMock.Verify(r => r.GetDataForScheduler(), Times.Once);
            _repoMock.Verify(r => r.SaveSchedule("user-123", It.IsAny<ScheduleBlock?[,]>()), Times.Once);
        }

        [Fact]
        public async Task RunScheduler_NoAssignments_ThrowsValidationException()
        {
            // Arrange
            var data = GetSchedulerData();
            data.SetAssignments(new List<Assignment>());

            _repoMock.Setup(r => r.GetDataForScheduler())
                     .ReturnsAsync(data);

            // Act + Assert
            await Assert.ThrowsAsync<ValidationException>(() => _service.RunScheduler());

            _repoMock.Verify(r => r.SaveSchedule(It.IsAny<string>(), It.IsAny<ScheduleBlock?[,]>()), Times.Never);
        }

        [Fact]
        public async Task RunScheduler_NoCourses_ThrowsValidationException()
        {
            // Arrange
            var userId = "user-123";

            var data = GetSchedulerData();
            data.SetCourses(new List<Course>());

            _repoMock.Setup(r => r.GetDataForScheduler())
                     .ReturnsAsync(data);

            // Act + Assert
            await Assert.ThrowsAsync<ValidationException>(() => _service.RunScheduler());

            _repoMock.Verify(r => r.SaveSchedule(It.IsAny<string>(), It.IsAny<ScheduleBlock?[,]>()), Times.Never);
        }

        [Fact]
        public async Task RunScheduler_DailyCapacityTotals0_ThrowsValidationException()
        {
            // Arrange
            var zeroCapacities = new List<DailyCapacity>
            {
                new DailyCapacity("user-123", 0, 0),
                new DailyCapacity("user-123", 1, 0),
                new DailyCapacity("user-123", 2, 0),
                new DailyCapacity("user-123", 3, 0),
                new DailyCapacity("user-123", 4, 0),
                new DailyCapacity("user-123", 5, 0),
                new DailyCapacity("user-123", 6, 0)
            };
            var data = GetSchedulerData();
            data.SetDailyCapacities(zeroCapacities);

            _repoMock.Setup(r => r.GetDataForScheduler())
                     .ReturnsAsync(data);

            // Act + Assert
            await Assert.ThrowsAsync<ValidationException>(() => _service.RunScheduler());

            _repoMock.Verify(r => r.SaveSchedule(It.IsAny<string>(), It.IsAny<ScheduleBlock?[,]>()), Times.Never);
        }

        [Fact]
        public async Task RunScheduler_NoCourseWeights_ThrowsValidationException()
        {
            // Arrange
            var data = GetSchedulerData();
            data.SetCourseWeights(new List<CourseWeight>());

            _repoMock.Setup(r => r.GetDataForScheduler())
                     .ReturnsAsync(data);

            // Act + Assert
            await Assert.ThrowsAsync<ValidationException>(() => _service.RunScheduler());

            _repoMock.Verify(r => r.SaveSchedule(It.IsAny<string>(), It.IsAny<ScheduleBlock?[,]>()), Times.Never);
        }

        [Fact]
        public async Task RunScheduler_NoRules_ThrowsValidationException()
        {
            // Arrange
            var userId = "user-123";

            var data = new SchedulerData(
                assignments: new List<Assignment>
                {
                    new Assignment(userId, 1, "Assignment 1", null, DateTime.UtcNow.AddDays(3), null, false, false)
                },
                courses: new List<Course>
                {
                    new Course(userId, "CSC3032", "Course 1")
                },
                courseWeights: new List<CourseWeight>
                {
                    new CourseWeight(userId, 1, 100)
                },
                dailyCapacities: new List<DailyCapacity>
                {
                    new DailyCapacity(userId, 0, 2)
                },
                rules: null,
                avoidPeriods: new List<AvoidPeriod>()
            );

            _repoMock.Setup(r => r.GetDataForScheduler())
                     .ReturnsAsync(data);

            // Act + Assert
            await Assert.ThrowsAsync<ValidationException>(() => _service.RunScheduler());

            _repoMock.Verify(r => r.SaveSchedule(It.IsAny<string>(), It.IsAny<ScheduleBlock?[,]>()), Times.Never);
        }

        private SchedulerData GetSchedulerData()
        {
            return new SchedulerData(
                assignments: new List<Assignment>
                {
                    new Assignment("user-123", 0, "Assignment 1", null, DateTime.UtcNow.AddDays(3), null, false, false)
                },
                courses: new List<Course>
                {
                    new Course("user-123", "CSC3032", "Course 1")
                },
                courseWeights: new List<CourseWeight>
                {
                    new CourseWeight("user-123", 0, 100)
                },
                dailyCapacities: new List<DailyCapacity>
                {
                    new DailyCapacity("user-123", 0, 2),
                    new DailyCapacity("user-123", 1, 2),
                    new DailyCapacity("user-123", 2, 2),
                    new DailyCapacity("user-123", 3, 2),
                    new DailyCapacity("user-123", 4, 2),
                    new DailyCapacity("user-123", 5, 2),
                    new DailyCapacity("user-123", 6, 2)
                },
                rules: new SchedulerRules("user-123", 30, 120),
                avoidPeriods: new List<AvoidPeriod>()
            );
        }
    }
}
