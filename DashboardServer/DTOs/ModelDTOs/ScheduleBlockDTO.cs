namespace DashboardServer.DTOs.ModelDTOs
{
    public class ScheduleBlockDTO
    {
        public int ScheduleId { get; set; }
        public  int AssignmentId { get; set; }
        public string? AssignmentName { get; set; }
        public int CourseId { get; set; }
        public BlockType BlockType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

    }

    public enum BlockType
    {
        Assignment,
        Study
    }
}