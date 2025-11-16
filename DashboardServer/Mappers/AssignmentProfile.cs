using AutoMapper;
using DashboardServer.DTOs;
using DashboardServer.DTOs.ResponseDTOs;
using DashboardServer.Models;

namespace DashboardServer.Mappers
{
    public class AssignmentProfile : Profile
    {
        public AssignmentProfile()
        {
            CreateMap<AssignmentDTO, Assignment>();
            CreateMap<Assignment, AssignmentDTO>();
            CreateMap<CanvasAssignmentDTO, Assignment>()
                .ForMember(dest => dest.AssignmentId, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.course_id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.name));
                // .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.description))
                // .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.due_at))
                // .ForMember(dest => dest.HtmlUrl, opt => opt.MapFrom(src => src.html_url));
            CreateMap<Assignment, CanvasAssignmentDTO>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.AssignmentId))
                .ForMember(dest => dest.course_id, opt => opt.MapFrom(src => src.CourseId))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name));
                // .ForMember(dest => dest.description, opt => opt.MapFrom(src => src.Description))
                // .ForMember(dest => dest.due_at, opt => opt.MapFrom(src => src.DueDate))
                // .ForMember(dest => dest.html_url, opt => opt.MapFrom(src => src.HtmlUrl));
        }
    }   
}