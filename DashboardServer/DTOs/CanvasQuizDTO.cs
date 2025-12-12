namespace DashboardServer.DTOs
{
    public class CanvasQuizDTO
    {
        /// <summary>
        /// The unique identifier for the quiz.
        /// </summary>
        public int id { get; set; }
        public string title { get; set; } = string.Empty;
        public string html_url { get; set; } = string.Empty;
        public DateTimeOffset? due_at { get; set; }
        public DateTimeOffset? lock_at { get; set; }
    }
}