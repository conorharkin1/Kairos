using System.ComponentModel.DataAnnotations;

namespace DashboardServer.DTOs.ResponseDTOs
{
    public class CourseDTO
    {
        /// <summary>
        /// db identifier for the course.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The unique identifier for the course.
        /// </summary>
        public int ExternalCourseId { get; set; }
        
        /// <summary>
        /// The code of the course. eg. CSC3032
        /// </summary>
        [Required(ErrorMessage = "Course code is required")]
        public string? CourseCode { get; set; }

        [Required(ErrorMessage = "Course name is required")]
        public string? Name { get; set; }
    }
}