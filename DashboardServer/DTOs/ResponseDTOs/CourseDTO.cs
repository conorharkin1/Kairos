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
        public string? CourseId { get; set; }
        /// <summary>
        /// The code of the course. eg. CSC3032
        /// </summary>
        public string? CourseCode { get; set; }
        /// <summary>
        /// The name of the course.
        /// </summary>
        public string? Name { get; set; }
    }
}