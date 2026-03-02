namespace DashboardServer.Models
{
    public class Schedule : BaseEntity
    {
        protected Schedule()
        {
            Blocks = new List<ScheduleBlock>();
        }

        public Schedule(string userId) : base(userId)
        {
            Blocks = new List<ScheduleBlock>();
            CreatedAt = DateTime.UtcNow;
        }

        public ICollection<ScheduleBlock> Blocks { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    }
}