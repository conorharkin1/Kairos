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
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ExternalAssignmentId, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.course_id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.name))
                .ForMember(dest => dest.IsComplete, opt => opt.MapFrom(src => src.has_submitted_submissions))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.description))
                .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.due_at.HasValue ? src.due_at.Value.UtcDateTime : (DateTime?)null))
                .ForMember(dest => dest.HtmlUrl, opt => opt.MapFrom(src => src.html_url));
            CreateMap<Assignment, CanvasAssignmentDTO>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.ExternalAssignmentId))
                .ForMember(dest => dest.course_id, opt => opt.MapFrom(src => src.CourseId))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.description, opt => opt.Ignore())
                .ForMember(dest => dest.due_at, opt => opt.Ignore())
                .ForMember(dest => dest.html_url, opt => opt.Ignore());
        }
    }
}