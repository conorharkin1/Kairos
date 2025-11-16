using System.Net.Http.Headers;
using DashboardServer.DTOs;
using DashboardServer.Services.Interfaces;

namespace DashboardServer.Services
{
    public class CanvasApiService : ICanvasApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IEncryptService _encryptService;
        private readonly IUserService _userService;

        public CanvasApiService(HttpClient httpClient, IEncryptService encryptService, IUserService userService)
        {
            _encryptService = encryptService;
            _httpClient = httpClient;
            _userService = userService;
            _httpClient.BaseAddress = new Uri("https://canvas.qub.ac.uk/");
        }

        public async Task<List<CanvasCourseDTO>> GetFavoriteCoursesAsync(string userId)
        {
            await AddAuthHeaderAsync(userId);
            var courses = await _httpClient.GetFromJsonAsync<List<CanvasCourseDTO>>("/api/v1/users/self/favorites/courses");
            return courses ?? [];
        }

        public async Task<List<CanvasAssignmentDTO>> GetCourseAssignmentsAsync(string userId, int courseId)
        {
            await AddAuthHeaderAsync(userId);
            var assignments = await _httpClient.GetFromJsonAsync<List<CanvasAssignmentDTO>>($"/api/v1/courses/{courseId}/assignments");
            return assignments ?? [];
        }

        private async Task AddAuthHeaderAsync(string userId)
        {
            // for debugging
            var temp = await GetUserCanvasApiKey(userId);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetUserCanvasApiKey(userId));
        }

        private async Task<string> GetUserCanvasApiKey(string userId)
        {
            var cipherText = await _userService.GetEncryptedApiKey(userId);
            // again, using the userId to use as a key for decryption - not suitable for production but for demo it'll be fine
            Guid encryptionKey = Guid.Parse(userId);

            return _encryptService.Decrypt(cipherText, encryptionKey);
        }
        
    }
}