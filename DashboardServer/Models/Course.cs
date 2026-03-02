using DashboardServer.Exceptions;

namespace DashboardServer.Models
{
    public class Course : BaseEntity
    {
        public int? ExternalCourseId { get; private set; }
        public string CourseCode { get; private set; } = default!;
        public string Name { get; private set; } = default!;
        public ICollection<Assignment> Assignments { get; private set; }
        public ICollection<Quiz> Quizzes { get; private set; }

        protected Course()
        {
            Assignments = new List<Assignment>();
            Quizzes = new List<Quiz>();
        }

        public Course(string userId, string courseCode, string name) : base(userId)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ValidationException("Course name cannot be empty");
            }

            if (string.IsNullOrWhiteSpace(courseCode))
            {
                throw new ValidationException("Course code cannot be empty");
            }

            CourseCode = courseCode;
            Name = name;
            Assignments = new List<Assignment>();
            Quizzes = new List<Quiz>();
        }

        public void Update(string? courseCode, string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ValidationException("Course name cannot be empty");
            }

            if (string.IsNullOrWhiteSpace(courseCode))
            {
                throw new ValidationException("Course code cannot be empty");
            }

            CourseCode = courseCode;
            Name = name;
        }

        public void SetExternalCourseId(int? externalCourseId)
        {
            ExternalCourseId = externalCourseId;
        }

    }
}