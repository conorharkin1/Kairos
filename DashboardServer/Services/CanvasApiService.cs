using System.Net.Http.Headers;
using AutoMapper;
using DashboardServer.Data;
using DashboardServer.DTOs.CanvasDTOs;
using DashboardServer.Exceptions;
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
        private readonly IAssignmentService _assignmentService;
        private readonly IQuizService _quizService;
        private readonly ICourseService _courseService;
        private const string CANVAS_BASE_URL = "https://canvas.qub.ac.uk/";

        public CanvasApiService(HttpClient httpClient, IEncryptService encryptService, IUserService userService, ApplicationDbContext dbContext, IMapper mapper, IAssignmentService assignmentService, IQuizService quizService, ICourseService courseService)
        {
            _encryptService = encryptService;
            _httpClient = httpClient;
            _userService = userService;
            _dbContext = dbContext;
            _mapper = mapper;
            _assignmentService = assignmentService;
            _quizService = quizService;
            _courseService = courseService;
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
            try
            {
                var assignments = await _httpClient.GetFromJsonAsync<List<CanvasAssignmentDTO>>($"/api/v1/courses/{courseId}/assignments");
                return assignments ?? [];
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return [];
            }
        }

        private async Task<List<CanvasQuizDTO>> GetCourseQuizzesFromCanvasAsync(string userId, int courseId)
        {
            await AddAuthHeaderAsync(userId);
            try
            {
                var quizzes = await _httpClient.GetFromJsonAsync<List<CanvasQuizDTO>>($"/api/v1/courses/{courseId}/quizzes");
                return quizzes ?? [];
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return [];
            }
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

        /// <summary>
        /// Updates DB with Courses from Canvas, it's intentional to not update existing courses. This is becuase I do not want
        /// a user's custom changes to a Course to be over-written every time the data is updated. I feel it is 
        /// unlikely the name or code of the Course will ever change on Canvas therefore it would be inconvenient for the end user
        /// to have any custom changes made reset.    
        /// </summary>
        private async Task UpdateCoursesDataAsync(string userId)
        {
            var existingCanvasCourses = await _dbContext.Courses.Where(c => c.ExternalCourseId.HasValue).ToDictionaryAsync(c => c.ExternalCourseId!.Value, c => c);
            var incomingCanvasCourses = _mapper.Map<List<Course>>(await GetFavoriteCoursesFromCanvasAsync(userId));
            var coursesToAdd = new List<Course>();
            
            foreach (var course in incomingCanvasCourses)
            {
                if (!existingCanvasCourses.TryGetValue(course.ExternalCourseId!.Value, out var existingCourse))
                {
                    coursesToAdd.Add(course);
                }
                else
                {
                    continue;
                }
            }

            await _courseService.CreateCoursesAsync(userId, coursesToAdd);
        }

        /// <summary>
        /// Fetches Assignments from Canvas, compares to existing records. If any new assignments are present on Canvas,
        /// new records will be created in the database. Otherwise, if an assignment already exists, only the DueDate field will be updated.
        /// </summary>
        private async Task UpdateAssignmentsDataAsync(string userId)
        {
            var existingCourseIds = await _dbContext.Courses
                .Where(c => c.ExternalCourseId.HasValue)
                .Select(c => new { c.Id, c.ExternalCourseId }).ToListAsync();

            var existingAssignments = await _dbContext.Assignments.Where(a => a.ExternalAssignmentId.HasValue && a.ExternalAssignmentId > 0).ToListAsync();
            var existingAssignmentsLookup = existingAssignments.ToDictionary(a => a.ExternalAssignmentId!.Value, a => a);

            var assignmentsToAdd = new List<Assignment>();

            foreach(var existingCourse in existingCourseIds)
            {
                var incomingAssignments = _mapper.Map<List<Assignment>>(await GetCourseAssignmentsFromCanvasAsync(userId, existingCourse.ExternalCourseId!.Value));

                foreach (var assignment in incomingAssignments)
                {
                    if (!assignment.ExternalAssignmentId.HasValue)
                    {
                        continue;
                    }

                    if (!existingAssignmentsLookup.TryGetValue(assignment.ExternalAssignmentId!.Value, out var existingAssignment))
                    {
                        assignment.CourseId = existingCourse.Id;
                        assignmentsToAdd.Add(assignment);
                    }
                    else
                    {
                        existingAssignment.DueDate = assignment.DueDate;
                    }
                }
            }

            await _assignmentService.CreateAssignmentsAsync(userId, assignmentsToAdd);
            await _dbContext.SaveChangesAsync();
        }
        /// <summary>
        /// Fetches Quizzes from Canvas, compares to existing records. If any new quizzes are present on Canvas,
        /// new records will be created in the database. Otherwise, if a quiz already exists,
        /// </summary>
        private async Task UpdateQuizzesDataAsync(string userId)
        {
            var existingCourseIds = await _dbContext.Courses
                .Where(c => c.ExternalCourseId.HasValue)
                .Select(c => new { c.Id, c.ExternalCourseId }).ToListAsync();

            var existingQuizzes = await _dbContext.Quizzes.Where(q => q.ExternalQuizId.HasValue && q.ExternalQuizId > 0).ToListAsync();
            var existingQuizzesLookup = existingQuizzes.ToDictionary(q => q.ExternalQuizId!.Value, q => q);

            var quizzesToAdd = new List<Quiz>();

            foreach(var existingCourse in existingCourseIds)
            {
                var incomingQuizzes = _mapper.Map<List<Quiz>>(await GetCourseQuizzesFromCanvasAsync(userId, existingCourse.ExternalCourseId!.Value));

                foreach (var quiz in incomingQuizzes)
                {
                    if (!quiz.ExternalQuizId.HasValue)
                    {
                        continue;
                    }

                    if (!existingQuizzesLookup.TryGetValue(quiz.ExternalQuizId!.Value, out var existingQuiz))
                    {
                        quiz.CourseId = existingCourse.Id;
                        quizzesToAdd.Add(quiz);
                    }
                    else
                    {
                        existingQuiz.DueDate = quiz.DueDate;
                        existingQuiz.LockDate = quiz.LockDate;
                    }
                }
            }

            await _quizService.CreateQuizzesAsync(userId, quizzesToAdd);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateDataFromCanvasAsync(string userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if ((DateTime.UtcNow - user!.LastUpdatedData) < TimeSpan.FromMinutes(10))
            {
                throw new BadRequestException("Your data was updated less than 10 minutes ago, please wait before refreshing again.");
            }
            await UpdateCoursesDataAsync(userId);
            await UpdateAssignmentsDataAsync(userId);
            await UpdateQuizzesDataAsync(userId);

            user!.LastUpdatedData = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
        }
        
    }
}