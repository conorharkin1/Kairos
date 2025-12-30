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

        public async Task<List<Course>> GetCoursesAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedException("User is not authorized");
            }

            return await _courseRepository.GetCoursesAsync(userId);
        }

        public async Task<List<Assignment>> GetCourseAssignmentsAsync(string userId, int courseId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedException("User is not authorized");
            }

            return await _courseRepository.GetCourseAssignmentsAsync(userId, courseId);
        }

        public async Task<List<Quiz>> GetCourseQuizzesAsync(string userId, int courseId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedException("User is not authorized");
            }

            return await _courseRepository.GetCourseQuizzesAsync(userId, courseId);
        }
    }
}