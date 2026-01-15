namespace DashboardServer.Models.Constraints
{
    public class CourseWeight : BaseEntity
    {
        public int CourseId { get; set; }
        /// <summary>
        /// Weight of the course as a percentage
        /// </summary>
        public int Weight { get; set; }
        public string? CourseName { get; set; }
    }
}