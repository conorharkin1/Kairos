using AutoMapper;
using DashboardServer.DTOs.ConstraintDTOs;
using DashboardServer.Models.Constraints;

namespace DashboardServer.Mappers
{
    public class DailyCapacityProfile : Profile
    {
        public DailyCapacityProfile()
        {
            CreateMap<DailyCapacityDTO, DailyCapacity>();
            CreateMap<DailyCapacity, DailyCapacityDTO>();
        }
    }
}