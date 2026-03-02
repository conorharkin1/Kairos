using DashboardServer.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace DashboardServer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }
        public string? CanvasApiKey { get; private set; }
        public DateTime? LastUpdatedData { get; private set; }
        public Schedule? Schedule { get; private set; }

        public ApplicationUser() { }

        public ApplicationUser(string firstName, string lastName, string canvasApiKey) : base()
        {
            FirstName = firstName;
            LastName = lastName;
            CanvasApiKey = canvasApiKey;
        }

        public void UpdateLastSyncTime()
        {
            LastUpdatedData = DateTime.UtcNow;
        }

        public void SetApiKey(string encryptedApiKey)
        {
            if (string.IsNullOrWhiteSpace(encryptedApiKey))
            {
                throw new ValidationException("API key cannot be empty");
            }
            
            CanvasApiKey = encryptedApiKey;
        }
    }
}