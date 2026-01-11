namespace DashboardServer.Models.Constraints
{
    public class DailyAvailability : BaseEntity
    {
        /// <summary>
        /// 0 = Sunday 6 = Saturday
        /// </summary>
        public int DayIndex { get; set; }
        /// <summary>
        /// Maximum available hours for this day
        /// </summary>
        public int MaxHours { get; set; }
    }
}