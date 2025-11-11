using AutoMapper;
using DashboardServer.DTOs;
using DashboardServer.Models;
using DashboardServer.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DashboardServer.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UserDTO> RegisterAsync(RegisterDTO dto)
        {
            var user = _mapper.Map<ApplicationUser>(dto);
            user.UserName = dto.Email;

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                throw new Exception("User registration failed");
            }

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> LoginAsync(LoginDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                throw new UnauthorizedAccessException();
            }

            return _mapper.Map<UserDTO>(user);
        }
    }
}