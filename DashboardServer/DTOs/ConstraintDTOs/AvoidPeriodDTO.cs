namespace DashboardServer.DTOs.ConstraintDTOs
{
    public class AvoidPeriodDTO
    {
        public int Id { get; set; }
        /// <summary>
        /// 0 = Sunday 6 = Saturday
        /// </summary>
        public int DayIndex { get; set; }
        
        public List<AvoidTimeBlockDTO>? AvoidTimeBlocks { get; set; }
    }

    public class AvoidTimeBlockDTO
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}