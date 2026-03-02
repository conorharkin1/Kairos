using DashboardServer.Exceptions;

namespace DashboardServer.Models
{
    public class Quiz : BaseEntity
    {
        public int? ExternalQuizId { get; private set; }
        public int CourseId { get; private set; }
        public Course Course { get; private set; } = default!;
        public string Name { get; private set; } = default!;
        public DateTime? DueDate { get; private set; }
        public DateTime? LockDate { get; private set; }
        public string? HtmlUrl { get; private set; }
        public bool IsComplete { get; private set; }
        public bool IsPriority { get; private set; }

        protected Quiz() { }

        public Quiz(string userId, int courseId, string name, DateTime? dueDate, DateTime? lockDate, string? htmlUrl, bool isComplete, bool isPriority) : base(userId)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ValidationException("Quiz name cannot be empty");
            }

            if (courseId <= 0)
            {
                throw new ValidationException("Valid Course Id is required");
            }

            CourseId = courseId;
            Name = name;
            DueDate = dueDate;
            LockDate = lockDate;
            HtmlUrl = htmlUrl;
            IsComplete = isComplete;
            IsPriority = isPriority;
        }

        public void Update(string name, DateTime? dueDate, DateTime? lockDate, string? htmlUrl, bool isComplete, bool isPriority)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ValidationException("Quiz name cannot be empty");
            }

            Name = name;
            DueDate = dueDate;
            LockDate = lockDate;
            HtmlUrl = htmlUrl;
            IsComplete = isComplete;
            IsPriority = isPriority;
        }

        public void SetCourseId(int courseId)
        {
            if (courseId <= 0)
            {
                throw new ValidationException("Valid Course Id is required");
            }

            CourseId = courseId;
        }

        public void SetDueDate(DateTime? dueDate)
        {
            DueDate = dueDate;
        }

        public void SetLockDate(DateTime? lockDate)
        {
            LockDate = lockDate;
        }

        public void SetExternalQuizId(int externalQuizId)
        {
            ExternalQuizId = externalQuizId;
        }
    }
}