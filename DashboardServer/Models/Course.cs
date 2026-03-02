using DashboardServer.Exceptions;

namespace DashboardServer.Models
{
    public class Course : BaseEntity
    {
        public int? ExternalCourseId { get; private set; }
        public string CourseCode { get; private set; }
        public string Name { get; private set; }
        public ICollection<Assignment> Assignments { get; private set; }
        public ICollection<Quiz> Quizzes { get; private set; }

        protected Course()
        {
            Assignments = new List<Assignment>();
            Quizzes = new List<Quiz>();
        }

        public Course(string userId, string? courseCode, string? name) : base(userId)
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

        public void AddAssignment(Assignment assignment)
        {
            if (assignment == null)
            {
                throw new ValidationException("Assignment cannot be null");
            }

            if (assignment.CourseId != 0 && assignment.CourseId != Id)
            {
                throw new ValidationException("Assignment's CourseId does not match the Course Id");
            }

            assignment.SetCourseId(Id);
            Assignments.Add(assignment);
        }

        public void AddQuiz(Quiz quiz)
        {
            if (quiz == null)
            {
                throw new ValidationException("Quiz cannot be null");
            }

            if (quiz.CourseId != 0 && quiz.CourseId != Id)
            {
                throw new ValidationException("Quiz's CourseId does not match the Course Id");
            }

            quiz.SetCourseId(Id);
            Quizzes.Add(quiz);
        }

        public void RemoveAssignment(Assignment assignment)
        {
            if (assignment == null)
            {
                throw new ValidationException("Assignment cannot be null");
            }

            if (!Assignments.Contains(assignment))
            {
                throw new ValidationException("Assignment does not belong to this Course");
            }

            Assignments.Remove(assignment);
        }

        public void RemoveQuiz(Quiz quiz)
        {
            if (quiz == null)
            {
                throw new ValidationException("Quiz cannot be null");
            }

            if (!Quizzes.Contains(quiz))
            {
                throw new ValidationException("Quiz does not belong to this Course");
            }

            Quizzes.Remove(quiz);
        }

        public void SetExternalCourseId(int? externalCourseId)
        {
            ExternalCourseId = externalCourseId;
        }

    }
}