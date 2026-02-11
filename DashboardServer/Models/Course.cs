namespace DashboardServer.Models
{
    public class Course : BaseEntity
    {
        public Course()
        {
            Assignments = new List<Assignment>();
            Quizzes = new List<Quiz>();
        }

        public int? ExternalCourseId { get; set; }
        public string? CourseCode { get; set; }
        public string? Name { get; set; }
        public ICollection<Assignment> Assignments { get; set; }
        public ICollection<Quiz> Quizzes { get; set; }
    }
}