using Microsoft.AspNetCore.Identity;

namespace DashboardServer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CanvasApiKey { get; set; }
        public DateTime? LastUpdatedData { get; set; }
        public Schedule? Schedule { get; set; }
    }
}