using DashboardServer.Exceptions;

namespace DashboardServer.Models.Constraints
{
    public class AvoidTimeBlock
    {
        public int Id { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }

        protected AvoidTimeBlock() { }

        // ** Attribution **
        // Claude Opus 4.7 generated the logic for validating time blocks fall between 9am and 9pm.

        private static readonly TimeSpan SchedulingDayStart = new(9, 0, 0);
        private static readonly TimeSpan SchedulingDayEnd = new(21, 0, 0);

        public AvoidTimeBlock(TimeSpan startTime, TimeSpan endTime)
        {
            if (endTime <= startTime)
            {
                throw new ValidationException("End time must be after start time");
            }

            if (startTime < SchedulingDayStart || endTime > SchedulingDayEnd)
            {
                throw new ValidationException("Avoid periods must fall between 09:00 and 21:00");
            }

            StartTime = startTime;
            EndTime = endTime;
        }
    }
}