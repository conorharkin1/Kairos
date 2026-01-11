using DashboardServer.Data;
using DashboardServer.Models;
using DashboardServer.Repositories;
using DashboardServer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace DashboardServer.Tests.Repository
{
    public class CourseRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly CourseRepository _repository;
        private readonly IUserResolverService _userResolverService;

        public CourseRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var userResolverMock = new Mock<IUserResolverService>();
            userResolverMock.Setup(x => x.UserId).Returns("123");

            _userResolverService = userResolverMock.Object;
            _dbContext = new ApplicationDbContext(options, _userResolverService);
            _repository = new CourseRepository(_dbContext);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        #region GetCoursesAsync Tests

        [Fact]
        public async Task GetCoursesAsync_ReturnsAllCourses()
        {
            // Arrange
            var userId = "123";
            var courses = new List<Course>
            {
                new Course { Name = "Course 1", CourseCode = "CS101", UserId = userId },
                new Course { Name = "Course 2", CourseCode = "CS102", UserId = userId },
                new Course { Name = "Course 3", CourseCode = "CS103", UserId = userId }
            };

            await _dbContext.Courses.AddRangeAsync(courses);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetCoursesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Contains(result, c => c.Name == "Course 1");
            Assert.Contains(result, c => c.Name == "Course 2");
            Assert.Contains(result, c => c.Name == "Course 3");
            Assert.All(result, c => Assert.Equal(userId, c.UserId));
        }

        [Fact]
        public async Task GetCoursesAsync_ReturnsEmptyList_WhenNoCoursesExist()
        {
            // Act
            var result = await _repository.GetCoursesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region GetCourseAssignmentsAsync Tests

        [Fact]
        public async Task GetCourseAssignmentsAsync_ReturnsAssignmentsForCourse()
        {
            // Arrange
            var userId = "123";
            var courseId = 1;

            var course = new Course { Id = courseId, Name = "Test Course", UserId = userId };
            var assignments = new List<Assignment>
            {
                new Assignment { Name = "Assignment 1", CourseId = courseId, UserId = userId, DueDate = DateTime.Now },
                new Assignment { Name = "Assignment 2", CourseId = courseId, UserId = userId, DueDate = DateTime.Now },
                new Assignment { Name = "Other Assignment", CourseId = 2, UserId = userId, DueDate = DateTime.Now }
            };

            await _dbContext.Courses.AddAsync(course);
            await _dbContext.Assignments.AddRangeAsync(assignments);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetCourseAssignmentsAsync(courseId, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, a => Assert.Equal(courseId, a.CourseId));
            Assert.Contains(result, a => a.Name == "Assignment 1");
            Assert.Contains(result, a => a.Name == "Assignment 2");
        }

        [Fact]
        public async Task GetCourseAssignmentsAsync_ReturnsEmptyList_WhenNoAssignmentsExist()
        {
            // Arrange
            var courseId = 1;

            // Act
            var result = await _repository.GetCourseAssignmentsAsync(courseId, true);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region GetCourseQuizzesAsync Tests

        [Fact]
        public async Task GetCourseQuizzesAsync_ReturnsQuizzesForCourse()
        {
            // Arrange
            var userId = "123";
            var courseId = 1;

            var course = new Course { Id = courseId, Name = "Test Course", UserId = userId };
            var quizzes = new List<Quiz>
            {
                new Quiz { Name = "Quiz 1", CourseId = courseId, UserId = userId, DueDate = DateTime.Now },
                new Quiz { Name = "Quiz 2", CourseId = courseId, UserId = userId, DueDate = DateTime.Now },
                new Quiz { Name = "Other Quiz", CourseId = 2, UserId = userId, DueDate = DateTime.Now }
            };

            await _dbContext.Courses.AddAsync(course);
            await _dbContext.Quizzes.AddRangeAsync(quizzes);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetCourseQuizzesAsync(courseId, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, q => Assert.Equal(courseId, q.CourseId));
            Assert.Contains(result, q => q.Name == "Quiz 1");
            Assert.Contains(result, q => q.Name == "Quiz 2");
            Assert.All(result, q => Assert.Equal(userId, q.UserId));
        }

        [Fact]
        public async Task GetCourseQuizzesAsync_ReturnsEmptyList_WhenNoQuizzesExist()
        {
            // Arrange
            var courseId = 1;

            // Act
            var result = await _repository.GetCourseQuizzesAsync(courseId, true);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion
    }
}
