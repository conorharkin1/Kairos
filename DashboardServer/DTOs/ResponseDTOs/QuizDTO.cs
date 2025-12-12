using Microsoft.VisualBasic;

namespace DashboardServer.DTOs.ResponseDTOs
{
    public class QuizDTO
    {
        /// <summary>
        /// db identifier for the quiz.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The unique identifier for the quiz.
        /// </summary>
        public int ExternalQuizId { get; set; }
        /// <summary>
        /// Foreign key for the course this quiz belongs to.
        /// </summary>
        public int CourseId { get; set; }
        public string? Name { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? LockDate { get; set; }
        public string? HtmlUrl { get; set; }
    }
}