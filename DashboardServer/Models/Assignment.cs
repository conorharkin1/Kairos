using Microsoft.VisualBasic;

namespace DashboardServer.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public string? CourseId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DueDate DueDate { get; set; }
        public string? HtmlUrl { get; set; }
    }
}