using Microsoft.VisualBasic;

namespace DashboardServer.DTOs.ResponseDTOs
{
    public class AssignmentDTO
    {
        /// <summary>
        /// db identifier for the assignment.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The unique identifier for the assignment.
        /// </summary>
        public int AssignmentId { get; set; }
        /// <summary>
        /// Foreign key for the course this assignment belongs to.
        /// </summary>
        public int CourseId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DueDate DueDate { get; set; }
        public string? HtmlUrl { get; set; }
    }
}