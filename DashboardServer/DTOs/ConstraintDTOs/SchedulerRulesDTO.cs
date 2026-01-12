namespace DashboardServer.DTOs.ConstraintDTOs
{
    public class SchedulerRulesDTO
    {
        public int Id { get; set; }
        public int MinSessionLength { get; set; }
        public int MaxSessionLength { get; set; }
    }
}