namespace DashboardServer.DTOs.ConstraintDTOs.Responses
{
    public class AvoidPeriodResponseDTO
    {
        public int Id { get; set; }
        public int DayIndex { get; set; }
        public List<AvoidTimeBlockDTO>? AvoidTimeBlocks { get; set; }
    }
}
