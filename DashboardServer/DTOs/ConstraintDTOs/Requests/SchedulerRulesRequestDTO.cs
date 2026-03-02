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
    }
}
