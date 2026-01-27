namespace DashboardServer.Models.Constraints
{
    public class SchedulerRules : BaseEntity
    {
        /// <summary>
        /// Min length of each session 'block' in minutes
        /// </summary>
        public int MinSessionLength { get; set; } = 30;
        /// <summary>
        /// Max length of consecutive sessions in minutes
        /// </summary>
        public int MaxSessionLength { get; set; } = 180;
    }
}