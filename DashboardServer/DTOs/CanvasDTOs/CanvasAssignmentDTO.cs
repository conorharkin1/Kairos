using Microsoft.VisualBasic;

namespace DashboardServer.DTOs.CanvasDTOs
{
    public class CanvasAssignmentDTO
    {
        /// <summary>
        /// The unique identifier for the assignment.
        /// </summary>
        public int id { get; set; }
        public int course_id { get; set; }
        public string name { get; set; } = default!;
        public bool has_submitted_submissions { get; set; }
        public string? description { get; set; }
        public DateTimeOffset? due_at { get; set; }
        public string? html_url { get; set; }
    }
}