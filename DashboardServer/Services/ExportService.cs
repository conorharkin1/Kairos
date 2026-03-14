using System.Text;
using DashboardServer.DTOs.ModelDTOs;
using DashboardServer.Exceptions;
using DashboardServer.Services.Interfaces;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;

namespace DashboardServer.Services
{
    public class ExportService : IExportService
    {
        private readonly ISchedulerService _schedulerService;
        public ExportService(ISchedulerService schedulerService)
        {
            _schedulerService = schedulerService;
        }

        public async Task<byte[]> GetScheduleAsIcsAsync()
        {
            var schedule = await _schedulerService.GetSchedule();

            if (schedule == null)
            {
                throw new NotFoundException("Please create a schedule before exporting");
            }

            var calendar = new Calendar();
            calendar.Method = "PUBLISH";
            calendar.ProductId = "-//CHarkin40362646//Student Scheduler//EN";
            calendar.AddTimeZone(new VTimeZone("Europe/Dublin"));

            foreach(var block in schedule.Blocks)
            {
                calendar.Events.Add(MapToEvent(block));
            }

            var serializer = new CalendarSerializer();
            var serializedCalendar = serializer.SerializeToString(calendar);

            if (string.IsNullOrEmpty(serializedCalendar))
            {
                throw new Exception("An error occurred while generating the calendar file.");
            }

            return Encoding.UTF8.GetBytes(serializedCalendar);
        }

        private CalendarEvent MapToEvent(ScheduleBlockDTO block)
        {
            return new CalendarEvent
            {
                Uid = $"{block.BlockId}@studentscheduler",
                Summary = block.Name,
                Start = new CalDateTime(block.StartTime),
                End = new CalDateTime(block.EndTime),
                Created = new CalDateTime(DateTime.UtcNow),
                LastModified = new CalDateTime(DateTime.UtcNow),
            };
        }
    }
}