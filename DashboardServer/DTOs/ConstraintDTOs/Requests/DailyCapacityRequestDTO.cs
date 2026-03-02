using System.ComponentModel.DataAnnotations;

namespace DashboardServer.DTOs.ConstraintDTOs.Requests
{
    public class DailyCapacityRequestDTO
    {
        [Required]
        [Range(0, 6, ErrorMessage = "Day index must be between 0 (Sunday) and 6 (Saturday)")]
        public int DayIndex { get; set; }

        [Required]
        [Range(0, 24, ErrorMessage = "Max hours must be between 0 and 24")]
        public int MaxHours { get; set; }
    }
}
