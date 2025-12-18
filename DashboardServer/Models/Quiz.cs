namespace DashboardServer.Models
{
    public class Quiz : BaseEntity
    {
        public int? ExternalQuizId { get; set; }
        public int CourseId { get; set; }
        public Course? Course { get; set; }
        public string? Name { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? LockDate { get; set; }
        public string? HtmlUrl { get; set; }
        public bool IsComplete { get; set; }
        public bool IsFavourite { get; set; }
    }
}