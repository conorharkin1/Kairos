using AutoMapper;
using DashboardServer.DTOs.ConstraintDTOs;
using DashboardServer.Models.Constraints;

namespace DashboardServer.Mappers
{

    public class SchedulerRulesProfile : Profile
    {
        public SchedulerRulesProfile()
        {
            CreateMap<SchedulerRulesDTO, SchedulerRules>();
            CreateMap<SchedulerRules, SchedulerRulesDTO>();
        }
    }
}