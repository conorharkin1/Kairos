using System.ComponentModel.DataAnnotations;

namespace DashboardServer.DTOs.ModelDTOs.Requests
{
    public class QuizRequestDTO
    {
        [Required(ErrorMessage = "Course Id is required")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Quiz name is required")]
        public string Name { get; set; } = default!;
        public DateTime? DueDate { get; set; }
        public DateTime? LockDate { get; set; }
        public string? HtmlUrl { get; set; }
        public bool IsComplete { get; set; }
        public bool IsPriority { get; set; }
    }
}