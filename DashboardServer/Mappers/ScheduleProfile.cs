using AutoMapper;
using DashboardServer.DTOs.ModelDTOs;
using DashboardServer.Models;

namespace DashboardServer.Mappers
{

    public class ScheduleProfile : Profile
    {
        public ScheduleProfile()
        {
            CreateMap<Schedule, ScheduleDTO>();
            CreateMap<ScheduleBlock, ScheduleBlockDTO>();
        }
    }
}