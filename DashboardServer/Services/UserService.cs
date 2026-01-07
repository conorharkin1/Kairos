using DashboardServer.Exceptions;
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
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new BadRequestException("API key cannot be empty");
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedException("User is not authorized");
            }

            if (key == Guid.Empty)
            {
                throw new BadRequestException("Invalid encryption key");
            }

            var encryptedKey = _encryptService.Encrypt(apiKey, key);
            await _userRepository.SetApiKey(userId, encryptedKey);
        }

        public async Task<string> GetEncryptedApiKey(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedException("User is not authorized");
            }

            return await _userRepository.GetEncryptedApiKey(userId);
        }
    }
}