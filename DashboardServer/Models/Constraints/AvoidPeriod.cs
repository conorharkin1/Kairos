using DashboardServer.Exceptions;

namespace DashboardServer.Models.Constraints
{
    public class AvoidPeriod : BaseEntity
    {
        /// <summary>
        /// 0 = Sunday 6 = Saturday
        /// </summary>
        public int DayIndex { get; private set; }

        public List<AvoidTimeBlock> AvoidTimeBlocks { get; private set; }

        protected AvoidPeriod()
        {
            AvoidTimeBlocks = new List<AvoidTimeBlock>();
        }

        public AvoidPeriod(string userId, int dayIndex, List<AvoidTimeBlock>? avoidTimeBlocks = null) : base(userId)
        {
            if (dayIndex < 0 || dayIndex > 6)
            {
                throw new ValidationException("Day index must be between 0 (Sunday) and 6 (Saturday)");
            }

            DayIndex = dayIndex;
            AvoidTimeBlocks = avoidTimeBlocks ?? new List<AvoidTimeBlock>();
        }

        public void UpdateTimeBlocks(List<AvoidTimeBlock> avoidTimeBlocks)
        {
            AvoidTimeBlocks.Clear();
            foreach (var block in avoidTimeBlocks ?? [])
            {
                AvoidTimeBlocks.Add(block);
            }
        }
    }

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