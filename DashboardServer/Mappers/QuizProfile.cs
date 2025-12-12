using AutoMapper;
using DashboardServer.DTOs;
using DashboardServer.DTOs.ResponseDTOs;
using DashboardServer.Models;

namespace DashboardServer.Mappers
{
    public class QuizProfile : Profile
    {
        public QuizProfile()
        {
            CreateMap<QuizDTO, Quiz>();
            CreateMap<Quiz, QuizDTO>();
            CreateMap<CanvasQuizDTO, Quiz>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ExternalQuizId, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.CourseId, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.title))
                .ForMember(dest => dest.LockDate, opt => opt.MapFrom(src => src.lock_at.HasValue ? src.lock_at.Value.UtcDateTime : (DateTime?)null))
                .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.due_at.HasValue ? src.due_at.Value.UtcDateTime : (DateTime?)null))
                .ForMember(dest => dest.HtmlUrl, opt => opt.MapFrom(src => src.html_url));
        }
    }
}