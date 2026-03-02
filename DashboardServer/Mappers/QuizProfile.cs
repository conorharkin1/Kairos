using AutoMapper;
using DashboardServer.DTOs.ModelDTOs.Responses;
using DashboardServer.Models;

namespace DashboardServer.Mappers
{
    public class QuizProfile : Profile
    {
        public QuizProfile()
        {
            CreateMap<Quiz, QuizResponseDTO>();
        }
    }
}