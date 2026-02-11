namespace DashboardServer.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!;
    }
}