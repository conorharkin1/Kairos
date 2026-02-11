namespace DashboardServer.Models
{
    public class ScheduleBlock : BaseEntity
    {
        public int ScheduleId { get; set; }
        public Schedule? Schedule { get; set; }
        public double Priority { get; set; }
        public int? AssignmentId { get; set; }
        public string? AssignmentName { get; set; }
        public int CourseId { get; set; }
        public Course? Course { get; set; }
        public BlockType BlockType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

    }

    public enum BlockType
    {
        Assignment,
        Study,
        Break
    }
}