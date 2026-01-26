namespace DashboardServer.Models
{
    public class Schedule : BaseEntity
    {
        public ICollection<ScheduleBlock> Blocks { get; set; } = new List<ScheduleBlock>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}