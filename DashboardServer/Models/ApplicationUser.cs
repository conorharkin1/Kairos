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
        public bool IsOptedOutForEmails { get; private set; } = false;

        public ApplicationUser() { }

        public ApplicationUser(string firstName, string lastName) : base()
        {
            FirstName = firstName;
            LastName = lastName;
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

        public void SetEmailOptOut(bool optOut)
        {
            IsOptedOutForEmails = optOut;
        }
    }
}