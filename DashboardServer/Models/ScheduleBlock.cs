namespace DashboardServer.Models
{
    public class ScheduleBlock : BaseEntity
    {
        protected ScheduleBlock() { }

        public ScheduleBlock(string userId, int courseId, int? assignmentId, string name, string? assignmentName, double priority, BlockType blockType) : base(userId)
        {
            BlockId = Guid.NewGuid().ToString();
            Name = name;
            CourseId = courseId;
            AssignmentId = assignmentId;
            AssignmentName = assignmentName;
            Priority = priority;
            BlockType = blockType;
        }

        public string BlockId { get; private set; }
        public string Name { get; private set; }
        public int ScheduleId { get; private set; }
        public Schedule? Schedule { get; private set; }
        public double Priority { get; private set; }
        public int? AssignmentId { get; private set; }
        public string? AssignmentName { get; private set; }
        public int CourseId { get; private set; }
        public Course? Course { get; private set; }
        public BlockType BlockType { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public void SetSchedule(Schedule schedule)
        {
            Schedule = schedule;
        }

        public void SetTimeSlot(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public void SetPriority(double priority)
        {
            Priority = priority;
        }
    }

    public enum BlockType
    {
        Assignment,
        Study,
        Break
    }
}