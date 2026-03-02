using DashboardServer.Exceptions;

namespace DashboardServer.Models
{
    public abstract class BaseEntity
    {
        protected BaseEntity() { }

        protected BaseEntity(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ValidationException("UserId cannot be empty");
            }

            UserId = userId;
        }

        public int Id { get; private set; }
        public string UserId { get; private set; } = default!;
        public ApplicationUser User { get; private set; } = default!;
    }
}