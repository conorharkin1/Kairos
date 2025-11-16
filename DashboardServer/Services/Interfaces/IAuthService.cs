using DashboardServer.DTOs;

namespace DashboardServer.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResultDTO> RegisterAsync(RegisterDTO dto);
        Task<AuthResultDTO> LoginAsync(LoginDTO dto);
        Task DeleteAccountAsync(string userId);
    }
}