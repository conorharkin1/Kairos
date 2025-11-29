using System.Net.Http.Headers;
using AutoMapper;
using DashboardServer.Data;
using DashboardServer.DTOs;
using DashboardServer.Models;
using DashboardServer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DashboardServer.Services
{
    public class CanvasApiService : ICanvasApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IEncryptService _encryptService;
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private const string CANVAS_BASE_URL = "https://canvas.qub.ac.uk/";

        public CanvasApiService(HttpClient httpClient, IEncryptService encryptService, IUserService userService, ApplicationDbContext dbContext, IMapper mapper)
        {
            _encryptService = encryptService;
            _httpClient = httpClient;
            _userService = userService;
            _dbContext = dbContext;
            _mapper = mapper;
            _httpClient.BaseAddress = new Uri(CANVAS_BASE_URL);
        }

        public async Task<List<CanvasCourseDTO>> GetFavoriteCoursesFromCanvasAsync(string userId)
        {
            await AddAuthHeaderAsync(userId);
            var courses = await _httpClient.GetFromJsonAsync<List<CanvasCourseDTO>>("/api/v1/users/self/favorites/courses");
            return courses ?? [];
        }

        public async Task<List<CanvasAssignmentDTO>> GetCourseAssignmentsFromCanvasAsync(string userId, int courseId)
        {
            await AddAuthHeaderAsync(userId);
            var assignments = await _httpClient.GetFromJsonAsync<List<CanvasAssignmentDTO>>($"/api/v1/courses/{courseId}/assignments");
            return assignments ?? [];
        }

        private async Task AddAuthHeaderAsync(string userId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetUserCanvasApiKey(userId));
        }

        private async Task<string> GetUserCanvasApiKey(string userId)
        {
            var cipherText = await _userService.GetEncryptedApiKey(userId);
            // again, using the userId to use as a key for decryption - not suitable for production but for demo it'll be fine
            Guid encryptionKey = Guid.Parse(userId);

            return _encryptService.Decrypt(cipherText, encryptionKey);
        }

        private async Task UpdateCoursesDataAsync(string userId)
        {
            var existingCanvasCourses = await _dbContext.Courses.Where(c => c.ExternalCourseId.HasValue).ToDictionaryAsync(c => c.ExternalCourseId.Value, c => c);
            var incomingCanvasCourses = _mapper.Map<List<Course>>(await GetFavoriteCoursesFromCanvasAsync(userId));
            
            foreach (var course in incomingCanvasCourses)
            {
                if (!existingCanvasCourses.TryGetValue(course.ExternalCourseId.Value, out var existingCourse))
                {
                    // Insert new
                    course.UserId = userId;
                    await _dbContext.Courses.AddAsync(course);
                }
                else
                {
                    // Update existing
                    _mapper.Map(course, existingCourse);
                }
            }

            await _dbContext.SaveChangesAsync();
        }

        private async Task UpdateAssignmentsDataAsync(string userId)
        {
            var dbCanvasCourses = await _dbContext.Courses.Where(c => c.ExternalCourseId.HasValue).ToListAsync();

            foreach(var course in dbCanvasCourses)
            {
                var existingCanvasAssignments = await _dbContext.Assignments.Where(a => a.CourseId == course.Id && a.ExternalAssignmentId.HasValue).ToDictionaryAsync(a => a.ExternalAssignmentId.Value, a => a);

                var incomingCanvasAssignments = _mapper.Map<List<Assignment>>(await GetCourseAssignmentsFromCanvasAsync(userId, course.ExternalCourseId.Value));

                foreach (var assignment in incomingCanvasAssignments)
                {
                    assignment.CourseId = course.Id;

                    if (!existingCanvasAssignments.TryGetValue(assignment.ExternalAssignmentId.Value, out var existingAssignment))
                    {
                        // Insert new
                        assignment.UserId = userId;
                        await _dbContext.Assignments.AddAsync(assignment);
                    }
                    else
                    {
                        // Update existing
                        _mapper.Map(assignment, existingAssignment);
                    }
                }
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateDataFromCanvasAsync(string userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (DateTime.UtcNow - user!.LastUpdatedData < TimeSpan.FromMinutes(10))
            {
                throw new BadHttpRequestException("Your data was updated less than 10 minutes ago, please wait before refreshing again.");
            }
            await UpdateCoursesDataAsync(userId);
            await UpdateAssignmentsDataAsync(userId);

            user!.LastUpdatedData = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
        }
        
    }
}