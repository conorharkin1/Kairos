namespace DashboardServer.DTOs.ModelDTOs.Responses
{
    public class CourseResponseDTO
    {
        /// <summary>
        /// db identifier for the course.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// The unique identifier for the course.
        /// </summary>
        public int? ExternalCourseId { get; set; }
        
        /// <summary>
        /// The code of the course. eg. CSC3032
        /// </summary>
        public string? CourseCode { get; set; }

        public string? Name { get; set; }
    }
}