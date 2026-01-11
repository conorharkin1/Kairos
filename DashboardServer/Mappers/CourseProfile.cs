using AutoMapper;
using DashboardServer.DTOs.CanvasDTOs;
using DashboardServer.DTOs.ModelDTOs;
using DashboardServer.Models;

namespace DashboardServer.Mappers
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<CourseDTO, Course>();
            CreateMap<Course, CourseDTO>();
            CreateMap<CanvasCourseDTO, Course>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.name))
                .ForMember(dest => dest.CourseCode, opt => opt.MapFrom(src => src.course_code))
                .ForMember(dest => dest.ExternalCourseId, opt => opt.MapFrom(src => src.id));
            CreateMap<Course, CanvasCourseDTO>()
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.course_code, opt => opt.MapFrom(src => src.CourseCode))
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.ExternalCourseId));
        }
    }   
}