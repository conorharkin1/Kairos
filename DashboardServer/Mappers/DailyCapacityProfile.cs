using AutoMapper;
using DashboardServer.DTOs.ConstraintDTOs.Responses;
using DashboardServer.Models.Constraints;

namespace DashboardServer.Mappers
{
    public class DailyCapacityProfile : Profile
    {
        public DailyCapacityProfile()
        {
            CreateMap<DailyCapacity, DailyCapacityResponseDTO>();
        }
    }
}