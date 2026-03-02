using AutoMapper;
using DashboardServer.DTOs.ConstraintDTOs.Responses;
using DashboardServer.Models.Constraints;

namespace DashboardServer.Mappers
{
    public class CourseWeightProfile : Profile
    {
        public CourseWeightProfile()
        {
            CreateMap<CourseWeight, CourseWeightResponseDTO>();
        }
    }
}