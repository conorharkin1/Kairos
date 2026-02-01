namespace DashboardServer.Models.Constraints
{
    public class AvoidPeriod : BaseEntity
    {
        /// <summary>
        /// 0 = Sunday 6 = Saturday
        /// </summary>
        public int DayIndex { get; set; }
        
        public List<AvoidTimeBlock>? AvoidTimeBlocks { get; set; }
        
    }

    public class AvoidTimeBlock
    {
        public int Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}