using AutoMapper;
using DashboardServer.DTOs.ModelDTOs.Responses;
using DashboardServer.Models;

namespace DashboardServer.Mappers
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseResponseDTO>();
        }
    }
}