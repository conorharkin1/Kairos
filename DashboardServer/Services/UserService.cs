using DashboardServer.Repositories.Interfaces;
using DashboardServer.Services.Interfaces;

namespace DashboardServer.Services
{
    public class UserService : IUserService
    {
        private readonly IEncryptService _encryptService;
        private readonly IUserRepository _userRepository;
        public UserService(IEncryptService encryptService, IUserRepository userRepository)
        {
            _encryptService = encryptService;
            _userRepository = userRepository;
        }

        public async Task SetApiKey(string apiKey, Guid key, string userId)
        {
            var encryptedKey = _encryptService.Encrypt(apiKey, key);
            await _userRepository.SetApiKey(userId, encryptedKey);
        }
    }
}