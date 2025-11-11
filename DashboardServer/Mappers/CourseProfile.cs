using AutoMapper;
using DashboardServer.DTOs;
using DashboardServer.Models;

public class CourseProfile : Profile
{
    public CourseProfile()
    {
        CreateMap<CanvasCourseDTO, Course>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id))
            .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.name))
            .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.course_code));
        CreateMap<Course, CanvasCourseDTO>()
            .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.CourseName))
            .ForMember(dest => dest.course_code, opt => opt.MapFrom(src => src.CourseId));
    }
}