using DashboardServer.DTOs;

namespace DashboardServer.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> RegisterAsync(RegisterDTO dto);
        Task<UserDTO> LoginAsync(LoginDTO dto);
    }
}