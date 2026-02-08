using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using DashboardServer.DTOs.AuthDTOs;
using DashboardServer.Models;
using DashboardServer.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using DashboardServer.Exceptions;

namespace DashboardServer.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public AuthService(UserManager<ApplicationUser> userManager, IMapper mapper, IConfiguration config)
        {
            _userManager = userManager;
            _mapper = mapper;
            _config = config;
        }

        public async Task<AuthResultDTO> RegisterAsync(RegisterDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                throw new BadRequestException("Email is required");
            }

            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new BadRequestException("Password is required");
            }

            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                throw new BadRequestException("A user with this email already exists");
            }

            var user = _mapper.Map<ApplicationUser>(dto);
            user.UserName = dto.Email;
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    throw new BadRequestException(error.Description);
                }
            }

            var token = GenerateJwtToken(user);

            return new AuthResultDTO
            {
                Token = token,
                User = _mapper.Map<UserDTO>(user)
            };
        }

        public async Task<AuthResultDTO> LoginAsync(LoginDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                throw new BadRequestException("Email is required");
            }

            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new BadRequestException("Password is required");
            }

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                throw new UnauthorizedException("Invalid email or password");
            }

            var token = GenerateJwtToken(user);
    
            return new AuthResultDTO
            {
                Token = token,
                User = _mapper.Map<UserDTO>(user)
            };
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim("uid", user.Id)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["JwtSettings:Key"] ?? throw new InvalidOperationException("JWT key not configured")));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task DeleteAccountAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedException("User is not authorized");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception("Failed to delete account");
            }
        }
    }
}