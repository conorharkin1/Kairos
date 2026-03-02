namespace DashboardServer.DTOs.ConstraintDTOs.Responses
{
    public class SchedulerRulesResponseDTO
    {
        public int Id { get; set; }
        public int MinSessionLength { get; set; }
        public int MaxSessionLength { get; set; }
    }
}
