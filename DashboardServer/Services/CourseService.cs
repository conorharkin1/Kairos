using DashboardServer.Exceptions;
using DashboardServer.Models;
using DashboardServer.Repositories.Interfaces;
using DashboardServer.Services.Interfaces;

namespace DashboardServer.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<Course> Create(string userId, Course course)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedException("User is not authorized");
            }

            if (string.IsNullOrWhiteSpace(course.Name))
            {
                throw new BadRequestException("Course name is required");
            }

            if (string.IsNullOrWhiteSpace(course.CourseCode))
            {
                throw new BadRequestException("Course code is required");
            }

            return await _courseRepository.Create(userId, course);
        }

        public async Task CreateCoursesAsync(string userId, List<Course> courses)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedException("User is not authorized");
            }

            await _courseRepository.CreateCoursesAsync(userId, courses);
        }

        public async Task<List<Course>> GetCoursesAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedException("User is not authorized");
            }

            return await _courseRepository.GetCoursesAsync();
        }

        public async Task<List<Assignment>> GetCourseAssignmentsAsync(string userId, int courseId, bool includeCompleted)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedException("User is not authorized");
            }

            if (courseId <= 0)
            {
                throw new BadRequestException("Invalid course ID");
            }

            return await _courseRepository.GetCourseAssignmentsAsync(courseId, includeCompleted);
        }

        public async Task<List<Quiz>> GetCourseQuizzesAsync(string userId, int courseId, bool includeCompleted)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedException("User is not authorized");
            }

            if (courseId <= 0)
            {
                throw new BadRequestException("Invalid course ID");
            }

            return await _courseRepository.GetCourseQuizzesAsync(courseId, includeCompleted);
        }
    }
}