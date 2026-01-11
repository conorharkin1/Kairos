using AutoMapper;
using DashboardServer.DTOs.AuthDTOs;
using DashboardServer.Models;

namespace DashboardServer.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterDTO, ApplicationUser>();
            CreateMap<ApplicationUser, UserDTO>()
            .ForMember(dest => dest.IsApiKeySet, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.CanvasApiKey)));
        }
    }
}