namespace DashboardServer.Models.Constraints
{
    public class SchedulerRules : BaseEntity
    {
        public int MinSessionLength { get; set; } = 30;
        public int MaxSessionLength { get; set; } = 180;
    }
}