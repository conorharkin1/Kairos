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

        public CourseRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var userResolverMock = new Mock<IUserResolverService>();
            userResolverMock.Setup(x => x.UserId).Returns("user-123");

            _dbContext = new ApplicationDbContext(options, userResolverMock.Object);
            _repository = new CourseRepository(_dbContext);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        private Course BuildCourse(string userId = "user-123", string courseCode = "CS101", string name = "Course 1")
        {
            return new Course(userId, courseCode, name);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllCourses()
        {
            // Arrange
            var userId = "user-123";
            var courses = new List<Course>
            {
                BuildCourse(userId, "CS101", "Course 1"),
                BuildCourse(userId, "CS102", "Course 2"),
                BuildCourse(userId, "CS103", "Course 3"),
            };

            await _dbContext.Courses.AddRangeAsync(courses);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Contains(result, c => c.Name == "Course 1");
            Assert.Contains(result, c => c.Name == "Course 2");
            Assert.Contains(result, c => c.Name == "Course 3");
            Assert.All(result, c => Assert.Equal(userId, c.UserId));
        }

        [Fact]
        public async Task GetAllAsync_ReturnsEmptyList_WhenNoCoursesExist()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetCourseAssignmentsAsync_ReturnsAssignmentsForCourse()
        {
            // Arrange
            var userId = "user-123";
            var course = BuildCourse(userId, "CS101", "Test Course");

            await _dbContext.Courses.AddAsync(course);
            await _dbContext.SaveChangesAsync();

            var assignments = new List<Assignment>
            {
                new Assignment(userId, course.Id, "Assignment 1", null, DateTime.UtcNow.AddDays(1), null, false, false),
                new Assignment(userId, course.Id, "Assignment 2", null, DateTime.UtcNow.AddDays(1), null, false, false),
                new Assignment(userId, 999, "Other Assignment", null, DateTime.UtcNow.AddDays(1), null, false, false)
            };

            await _dbContext.Assignments.AddRangeAsync(assignments);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetCourseAssignmentsAsync(course.Id, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, a => Assert.Equal(course.Id, a.CourseId));
            Assert.Contains(result, a => a.Name == "Assignment 1");
            Assert.Contains(result, a => a.Name == "Assignment 2");
        }

        [Fact]
        public async Task GetCourseAssignmentsAsync_ReturnsEmptyList_WhenNoAssignmentsExist()
        {
            // Arrange
            var course = BuildCourse();
            await _dbContext.Courses.AddAsync(course);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetCourseAssignmentsAsync(course.Id, true);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetCourseQuizzesAsync_ReturnsQuizzesForCourse()
        {
            // Arrange
            var userId = "user-123";
            var course = BuildCourse(userId, "CS101", "Test Course");

            await _dbContext.Courses.AddAsync(course);
            await _dbContext.SaveChangesAsync();

            var quizzes = new List<Quiz>
            {
                new Quiz(userId, course.Id, "Quiz 1", DateTime.UtcNow.AddDays(1), null, null, false, false),
                new Quiz(userId, course.Id, "Quiz 2", DateTime.UtcNow.AddDays(1), null, null, false, false),
                new Quiz(userId, 999, "Other Quiz", DateTime.UtcNow.AddDays(1), null, null, false, false)
            };

            await _dbContext.Quizzes.AddRangeAsync(quizzes);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetCourseQuizzesAsync(course.Id, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, q => Assert.Equal(course.Id, q.CourseId));
            Assert.Contains(result, q => q.Name == "Quiz 1");
            Assert.Contains(result, q => q.Name == "Quiz 2");
            Assert.All(result, q => Assert.Equal(userId, q.UserId));
        }

        [Fact]
        public async Task GetCourseQuizzesAsync_ReturnsEmptyList_WhenNoQuizzesExist()
        {
            // Arrange
            var course = BuildCourse();
            await _dbContext.Courses.AddAsync(course);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetCourseQuizzesAsync(course.Id, true);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task DeleteAsync_DeletesAssignmentsAndQuizzes_WhenCourseDeleted()
        {
            // Arrange
            var userId = "user-123";
            var course = BuildCourse(userId, "CS101", "Test Course");
            var otherCourse = BuildCourse(userId, "CS102", "Other Course");

            await _dbContext.Courses.AddRangeAsync(course, otherCourse);
            await _dbContext.SaveChangesAsync();

            var assignments = new List<Assignment>
            {
                new Assignment(userId, course.Id, "Assignment 1", null, DateTime.UtcNow.AddDays(1), null, false, false),
                new Assignment(userId, course.Id, "Assignment 2", null, DateTime.UtcNow.AddDays(1), null, false, false),
                new Assignment(userId, otherCourse.Id, "Other Assignment", null, DateTime.UtcNow.AddDays(1), null, false, false)
            };

            var quizzes = new List<Quiz>
            {
                new Quiz(userId, course.Id, "Quiz 1", DateTime.UtcNow.AddDays(1), null, null, false, false),
                new Quiz(userId, otherCourse.Id, "Other Quiz", DateTime.UtcNow.AddDays(1), null, null, false, false)
            };

            await _dbContext.Assignments.AddRangeAsync(assignments);
            await _dbContext.Quizzes.AddRangeAsync(quizzes);
            await _dbContext.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(course);

            // Assert
            var deletedCourse = await _dbContext.Courses.FindAsync(course.Id);
            Assert.Null(deletedCourse);

            var remainingAssignments = await _dbContext.Assignments.ToListAsync();
            var remainingQuizzes = await _dbContext.Quizzes.ToListAsync();

            Assert.DoesNotContain(remainingAssignments, a => a.CourseId == course.Id);
            Assert.Single(remainingAssignments); // Only assignments from the other course should remain

            Assert.DoesNotContain(remainingQuizzes, q => q.CourseId == course.Id);
            Assert.Single(remainingQuizzes); // Only quizzes from the other course should remain
        }
    }
}
