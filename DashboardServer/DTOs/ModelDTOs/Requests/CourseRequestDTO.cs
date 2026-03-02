using System.ComponentModel.DataAnnotations;

namespace DashboardServer.DTOs.ModelDTOs.Requests
{
    public class CourseRequestDTO
    {
        public int? ExternalCourseId { get; set; }
        
        /// <summary>
        /// The code of the course. eg. CSC3032
        /// </summary>
        [Required(ErrorMessage = "Course code is required")]
        public string? CourseCode { get; set; }

        [Required(ErrorMessage = "Course name is required")]
        public string? Name { get; set; }
    }
}