namespace DashboardServer.DTOs.ModelDTOs
{
    public class ScheduleDTO
    {
        public ICollection<ScheduleBlockDTO> Blocks { get; set; } = new List<ScheduleBlockDTO>();
        public DateTime CreatedAt { get; set; }
    }
}