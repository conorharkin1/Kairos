using AutoMapper;
using DashboardServer.DTOs.ModelDTOs.Responses;
using DashboardServer.Models;

namespace DashboardServer.Mappers
{
    public class AssignmentProfile : Profile
    {
        public AssignmentProfile()
        {
            CreateMap<Assignment, AssignmentResponseDTO>();
        }
    }
}