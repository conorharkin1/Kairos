using DashboardServer.Exceptions;

namespace DashboardServer.Models
{
    public class Assignment : BaseEntity
    {
        public int? ExternalAssignmentId { get; private set; }
        public int CourseId { get; private set; }
        public Course Course { get; private set; } = default!;
        public string Name { get; private set; } = default!;
        public string? Description { get; private set; }
        public DateTime? DueDate { get; private set; }
        public string? HtmlUrl { get; private set; }
        public bool IsComplete { get; private set; }
        public bool IsPriority { get; private set; }

        protected Assignment() { }

        public Assignment(string userId, int courseId, string name, string? description, DateTime? dueDate, string? htmlUrl, bool isComplete, bool isPriority) : base(userId)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ValidationException("Assignment name cannot be empty");
            }

            if (courseId <= 0)
            {
                throw new ValidationException("Valid Course Id is required");
            }

            CourseId = courseId;
            Name = name;
            Description = description;
            DueDate = dueDate;
            HtmlUrl = htmlUrl;
            IsComplete = isComplete;
            IsPriority = isPriority;
        }

        public void Update(string name, string? description, DateTime? dueDate, string? htmlUrl, bool isComplete, bool isPriority)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ValidationException("Assignment name cannot be empty");
            }

            Name = name;
            Description = description;
            DueDate = dueDate;
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

        public void SetCompletion(bool isComplete)
        {
            IsComplete = isComplete;
        }

        public void SetPriority(bool isPriority)
        {
            IsPriority = isPriority;
        }

        public void SetDueDate(DateTime? dueDate)
        {
            DueDate = dueDate;
        }

        public void SetExternalAssignmentId(int externalAssignmentId)
        {
            ExternalAssignmentId = externalAssignmentId;
        }
    }
}