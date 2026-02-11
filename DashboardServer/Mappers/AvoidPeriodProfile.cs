using AutoMapper;
using DashboardServer.DTOs.ConstraintDTOs;
using DashboardServer.Models.Constraints;

namespace DashboardServer.Mappers
{
    public class AvoidPeriodProfile : Profile
    {
        public AvoidPeriodProfile()
        {
            CreateMap<AvoidPeriodDTO, AvoidPeriod>();
            CreateMap<AvoidPeriod, AvoidPeriodDTO>();
            CreateMap<AvoidTimeBlockDTO, AvoidTimeBlock>();
            CreateMap<AvoidTimeBlock, AvoidTimeBlockDTO>();
        }

    }
}