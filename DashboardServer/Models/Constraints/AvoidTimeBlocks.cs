using DashboardServer.Exceptions;

namespace DashboardServer.Models.Constraints
{
    public class AvoidTimeBlock
    {
        public int Id { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }

        protected AvoidTimeBlock() { }

        public AvoidTimeBlock(TimeSpan startTime, TimeSpan endTime)
        {
            if (endTime <= startTime)
            {
                throw new ValidationException("End time must be after start time");
            }

            StartTime = startTime;
            EndTime = endTime;
        }
    }
}