using System.ComponentModel.DataAnnotations;

namespace DashboardServer.DTOs.ModelDTOs
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
        public int ExternalAssignmentId { get; set; }
        public int CourseId { get; set; }
        
        [Required(ErrorMessage = "Assignment name is required")]
        public string? Name { get; set; }
        
        [Required(ErrorMessage = "Due date is required")]
        public DateTime? DueDate { get; set; }
        public string? Description { get; set; }
        public string? HtmlUrl { get; set; }
        public bool IsComplete { get; set; }
        public bool IsPriority { get; set; }
    }
}