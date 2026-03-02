using AutoMapper;
using DashboardServer.DTOs.ConstraintDTOs;
using DashboardServer.DTOs.ConstraintDTOs.Responses;
using DashboardServer.Models.Constraints;

namespace DashboardServer.Mappers
{
    public class AvoidPeriodProfile : Profile
    {
        public AvoidPeriodProfile()
        {
            CreateMap<AvoidPeriod, AvoidPeriodResponseDTO>();
            CreateMap<AvoidTimeBlock, AvoidTimeBlockDTO>();
        }
    }
}