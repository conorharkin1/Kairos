namespace DashboardServer.DTOs.ModelDTOs.Responses
{
    public class QuizResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public int CourseId { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? LockDate { get; set; }
        public string? HtmlUrl { get; set; }
        public bool IsComplete { get; set; }
        public bool IsPriority { get; set; }
    }
}