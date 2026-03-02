namespace DashboardServer.DTOs.ModelDTOs.Responses
{
    public class AssignmentResponseDTO
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public string? HtmlUrl { get; set; }
        public bool IsComplete { get; set; }
        public bool IsPriority { get; set; }
    }
}