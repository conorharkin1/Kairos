using AutoMapper;
using DashboardServer.DTOs.ModelDTOs.Requests;
using DashboardServer.DTOs.ModelDTOs.Responses;
using DashboardServer.Exceptions;
using DashboardServer.Models;
using DashboardServer.Repositories.Interfaces;
using DashboardServer.Services.Interfaces;

namespace DashboardServer.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repo;
        private readonly IMapper _mapper;

        public CourseService(ICourseRepository courseRepository, IMapper mapper)
        {
            _repo = courseRepository;
            _mapper = mapper;
        }

        public async Task<List<CourseResponseDTO>> GetAllAsync()
        {
            var result = await _repo.GetAllAsync();
            return _mapper.Map<List<CourseResponseDTO>>(result);
        }

        public async Task<CourseResponseDTO> CreateAsync(string userId, CourseRequestDTO dto)
        {
            Course course = new Course(userId, dto.CourseCode, dto.Name);

            var createdCourse = await _repo.CreateAsync(course);
            return _mapper.Map<CourseResponseDTO>(createdCourse);
        }

        public async Task CreateBulkAsync(string userId, List<CourseRequestDTO> courses)
        {
            List<Course> coursesToCreate = courses.Select(dto => new Course(userId, dto.CourseCode, dto.Name)).ToList();
            await _repo.CreateBulkAsync(coursesToCreate);
        }

        public async Task<CourseResponseDTO> UpdateAsync(string userId, int courseId, CourseRequestDTO dto)
        {
            Course course = await _repo.GetByIdAsync(courseId) ?? throw new NotFoundException("Course does not exist");

            if (course.UserId != userId)
            {
                throw new UnAuthorizedException("User is not authorized to update this course");
            }

            course.Update(dto.CourseCode, dto.Name);

            await _repo.Save();
            return _mapper.Map<CourseResponseDTO>(course);
        }

        public async Task DeleteAsync(string userId, int courseId)
        {
            var course = await _repo.GetByIdAsync(courseId) ?? throw new NotFoundException("Course does not exist");

            if (course.UserId != userId)
            {
                throw new UnAuthorizedException("User is not authorized to delete this course");
            }

            await _repo.DeleteAsync(course);
        }

        public async Task<List<AssignmentResponseDTO>> GetCourseAssignmentsAsync(int courseId, bool includeCompleted)
        {
            if (courseId <= 0)
            {
                throw new BadRequestException("Invalid course ID");
            }

            var result = await _repo.GetCourseAssignmentsAsync(courseId, includeCompleted);
            return _mapper.Map<List<AssignmentResponseDTO>>(result);
        }

        public async Task<List<QuizResponseDTO>> GetCourseQuizzesAsync(int courseId, bool includeCompleted)
        {
            if (courseId <= 0)
            {
                throw new BadRequestException("Invalid course ID");
            }

            var result = await _repo.GetCourseQuizzesAsync(courseId, includeCompleted);
            return _mapper.Map<List<QuizResponseDTO>>(result);
        }
    }
}