using System.Net.Http.Headers;
using DashboardServer.Data;
using DashboardServer.DTOs.CanvasDTOs;
using DashboardServer.Exceptions;
using DashboardServer.Models;
using DashboardServer.Repositories.Interfaces;
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
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IQuizRepository _quizRepository;
        private readonly ICourseRepository _courseRepository;
        private const string CANVAS_BASE_URL = "https://canvas.qub.ac.uk/";

        public CanvasApiService(HttpClient httpClient, IEncryptService encryptService, IUserService userService, ApplicationDbContext dbContext, IAssignmentRepository assignmentRepository, IQuizRepository quizRepository, ICourseRepository courseRepository)
        {
            _encryptService = encryptService;
            _httpClient = httpClient;
            _userService = userService;
            _dbContext = dbContext;
            _assignmentRepository = assignmentRepository;
            _quizRepository = quizRepository;
            _courseRepository = courseRepository;
            _httpClient.BaseAddress = new Uri(CANVAS_BASE_URL);
        }

        private async Task<List<CanvasCourseDTO>> GetFavoriteCoursesFromCanvasAsync(string userId)
        {
            await AddAuthHeaderAsync(userId);
            var courses = await _httpClient.GetFromJsonAsync<List<CanvasCourseDTO>>("/api/v1/users/self/favorites/courses");
            return courses ?? [];
        }

        private async Task<List<CanvasAssignmentDTO>> GetCourseAssignmentsFromCanvasAsync(string userId, int courseId)
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
            var incomingCanvasCourses = await GetFavoriteCoursesFromCanvasAsync(userId);
            var coursesToAdd = new List<Course>();

            foreach (var canvasCourse in incomingCanvasCourses)
            {
                if (!existingCanvasCourses.ContainsKey(canvasCourse.id))
                {
                    var newCourse = new Course(userId, canvasCourse.course_code, canvasCourse.name);
                    newCourse.SetExternalCourseId(canvasCourse.id);
                    coursesToAdd.Add(newCourse);
                }
            }

            await _courseRepository.CreateBulkAsync(coursesToAdd);
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
                var incomingAssignments = await GetCourseAssignmentsFromCanvasAsync(userId, existingCourse.ExternalCourseId!.Value);

                if (incomingAssignments == null)
                {
                    continue;
                }

                foreach (var assignment in incomingAssignments)
                {
                    var due = assignment.due_at.HasValue ? assignment.due_at.Value.UtcDateTime : (DateTime?)null;
                    if (existingAssignmentsLookup.TryGetValue(assignment.id, out var existingAssignment))
                    {
                        existingAssignment.SetDueDate(due);
                    }
                    else
                    {
                        var newAssignment = new Assignment(userId, existingCourse.Id, assignment.name, assignment.description, due, assignment.html_url, assignment.has_submitted_submissions, false);
                        newAssignment.SetExternalAssignmentId(assignment.id);
                        assignmentsToAdd.Add(newAssignment);
                    }
                }
            }

            await _assignmentRepository.CreateBulkAsync(assignmentsToAdd);
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
                var incomingQuizzes = await GetCourseQuizzesFromCanvasAsync(userId, existingCourse.ExternalCourseId!.Value);

                foreach (var canvasQuiz in incomingQuizzes)
                {
                    var dueDate = canvasQuiz.due_at.HasValue ? canvasQuiz.due_at.Value.UtcDateTime : (DateTime?)null;
                    var lockDate = canvasQuiz.lock_at.HasValue ? canvasQuiz.lock_at.Value.UtcDateTime : (DateTime?)null;

                    if (existingQuizzesLookup.TryGetValue(canvasQuiz.id, out var existingQuiz))
                    {
                        existingQuiz.SetDueDate(dueDate);
                        existingQuiz.SetLockDate(lockDate);
                    }
                    else
                    {
                        var newQuiz = new Quiz(userId, existingCourse.Id, canvasQuiz.title, dueDate, lockDate, canvasQuiz.html_url, false, false);
                        newQuiz.SetExternalQuizId(canvasQuiz.id);
                        quizzesToAdd.Add(newQuiz);
                    }
                }
            }

            await _quizRepository.CreateBulkAsync(quizzesToAdd);
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

            user.UpdateLastSyncTime();
            await _dbContext.SaveChangesAsync();
        }
        
    }
}