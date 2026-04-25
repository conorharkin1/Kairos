using System.ComponentModel.DataAnnotations;

namespace DashboardServer.DTOs.ConstraintDTOs.Requests
{
    public class SchedulerRulesRequestDTO
    {
        [Required]
        [Range(30, 240, ErrorMessage = "Min session length must be between 30 and 240 minutes")]
        public int MinSessionLength { get; set; }

        [Required]
        [Range(30, 240, ErrorMessage = "Max session length must be between 30 and 240 minutes")]
        public int MaxSessionLength { get; set; }

        [Range(0.0, 1.0, ErrorMessage = "Deadline urgency weighting must be between 0 and 1")]
        public double DeadlineUrgencyWeighting { get; set; }

        [Range(0.0, 1.0, ErrorMessage = "Course preference weighting must be between 0 and 1")]
        public double CoursePreferenceWeighting { get; set; }
    }
}
