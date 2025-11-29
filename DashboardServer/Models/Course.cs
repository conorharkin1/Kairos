namespace DashboardServer.Models
{
    public class Course : BaseEntity
    {
        public int? ExternalCourseId { get; set; }
        public string? CourseCode { get; set; }
        public string? Name { get; set; }
        public ICollection<Assignment>? Assignments { get; set; }
    }
}