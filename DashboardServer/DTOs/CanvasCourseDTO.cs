namespace DashboardServer.DTOs
{
    public class CanvasCourseDTO
    {
        /// <summary>
        /// The unique identifier for the course.
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// The code of the course. eg. CSC3032
        /// </summary>
        public string course_code { get; set; } = string.Empty;
        /// <summary>
        /// The name of the course.
        /// </summary>
        public string name { get; set; } = string.Empty;
    }
}