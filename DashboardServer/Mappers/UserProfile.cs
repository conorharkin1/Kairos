using AutoMapper;
using DashboardServer.DTOs;
using DashboardServer.Models;

namespace DashboardServer.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterDTO, ApplicationUser>();
            CreateMap<ApplicationUser, UserDTO>();
        }
    }
}