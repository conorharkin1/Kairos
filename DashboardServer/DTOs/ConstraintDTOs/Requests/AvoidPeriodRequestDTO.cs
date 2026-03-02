using System.ComponentModel.DataAnnotations;

namespace DashboardServer.DTOs.ConstraintDTOs.Requests
{
    public class AvoidPeriodRequestDTO
    {
        [Required]
        [Range(0, 6, ErrorMessage = "Day index must be between 0 (Sunday) and 6 (Saturday)")]
        public int DayIndex { get; set; }

        public List<AvoidTimeBlockDTO>? AvoidTimeBlocks { get; set; }
    }
}
