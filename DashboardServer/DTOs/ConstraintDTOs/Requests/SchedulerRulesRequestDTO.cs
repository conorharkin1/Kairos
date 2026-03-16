using System.ComponentModel.DataAnnotations;

namespace DashboardServer.DTOs.ConstraintDTOs.Requests
{
    public class SchedulerRulesRequestDTO
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Min session length must be a positive integer")]
        public int MinSessionLength { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Max session length must be a positive integer")]
        public int MaxSessionLength { get; set; }

        [Range(0.0, 1.0, ErrorMessage = "Deadline urgency weighting must be between 0 and 1")]
        public double DeadlineUrgencyWeighting { get; set; }

        [Range(0.0, 1.0, ErrorMessage = "Course preference weighting must be between 0 and 1")]
        public double CoursePreferenceWeighting { get; set; }
    }
}
