namespace DashboardServer.Models
{
    public class ScheduleBlock
    {
        public double Priority { get; set; }
        public  int AssignmentId { get; set; }
        public int CourseId { get; set; }
        public BlockType BlockType { get; set; }

    }

    public enum BlockType
    {
        Assignment,
        Study
    }
}