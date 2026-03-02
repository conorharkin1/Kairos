using AutoMapper;
using DashboardServer.DTOs.ConstraintDTOs.Responses;
using DashboardServer.Models.Constraints;

namespace DashboardServer.Mappers
{
    public class SchedulerRulesProfile : Profile
    {
        public SchedulerRulesProfile()
        {
            CreateMap<SchedulerRules, SchedulerRulesResponseDTO>();
        }
    }
}