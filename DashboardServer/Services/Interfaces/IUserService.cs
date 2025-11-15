using DashboardServer.DTOs;

namespace DashboardServer.Services.Interfaces
{
    public interface IUserService
    {
        Task<AuthResultDTO> RegisterAsync(RegisterDTO dto);
        Task<AuthResultDTO> LoginAsync(LoginDTO dto);
    }
}