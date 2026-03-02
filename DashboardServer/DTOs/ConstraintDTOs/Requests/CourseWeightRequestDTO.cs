using System.ComponentModel.DataAnnotations;

namespace DashboardServer.DTOs.ConstraintDTOs.Requests
{
    public class CourseWeightRequestDTO
    {
        [Required(ErrorMessage = "Course Id is required")]
        public int CourseId { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Weight must be between 0 and 100")]
        public int Weight { get; set; }
    }
}
