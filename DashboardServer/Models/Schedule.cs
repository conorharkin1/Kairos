namespace DashboardServer.Models
{
    public class Schedule : BaseEntity
    {
        public Schedule()
        {
            Blocks = new List<ScheduleBlock>();
        }

        public ICollection<ScheduleBlock> Blocks { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}