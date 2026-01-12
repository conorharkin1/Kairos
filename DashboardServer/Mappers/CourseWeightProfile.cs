using AutoMapper;
using DashboardServer.DTOs.ConstraintDTOs;
using DashboardServer.Models.Constraints;

namespace DashboardServer.Mappers
{

    public class CourseWeightProfile : Profile
    {
        public CourseWeightProfile()
        {
            CreateMap<CourseWeightDTO, CourseWeight>();
            CreateMap<CourseWeight, CourseWeightDTO>();
        }
    }
}