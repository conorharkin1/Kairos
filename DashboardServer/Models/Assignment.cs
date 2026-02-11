using Microsoft.VisualBasic;

namespace DashboardServer.Models
{
    public class Assignment : BaseEntity
    {
        public int? ExternalAssignmentId { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; } = default!;

        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public string? HtmlUrl { get; set; }
        
        public bool IsComplete { get; set; }
        public bool IsPriority { get; set; }
    }
}