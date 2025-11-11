using DashboardServer.Models;
using DashboardServer.Repositories.Interfaces;
using DashboardServer.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DashboardServer.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IEncryptService _encryptService;
        private readonly IProfileRepository _profileRepository;
        public ProfileService(IEncryptService encryptService, IProfileRepository profileRepository)
        {
            _encryptService = encryptService;
            _profileRepository = profileRepository;
        }

        public async Task SetApiKey(string userId, string apiKey)
        {
            var encryptedKey = _encryptService.Encrypt(apiKey, userId, userId);
            await _profileRepository.SetApiKey(userId, encryptedKey);
        }
    }
}