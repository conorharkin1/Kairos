using System.ComponentModel.DataAnnotations;

namespace DashboardServer.DTOs.ModelDTOs.Requests
{
    public class AssignmentRequestDTO
    {
        [Required(ErrorMessage = "Course Id is required")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Assignment name is required")]
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public string? HtmlUrl { get; set; }
        public bool IsComplete { get; set; }
        public bool IsPriority { get; set; }
    }
}