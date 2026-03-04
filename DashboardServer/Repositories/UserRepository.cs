namespace DashboardServer.Repositories
{
    using DashboardServer.Models;
    using DashboardServer.Repositories.Interfaces;
    using DashboardServer.Data;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SetApiKey(string userId, string encryptedApiKey)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user != null)
            {
                user.SetApiKey(encryptedApiKey);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<string> GetEncryptedApiKey(string userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user != null && !String.IsNullOrEmpty(user.CanvasApiKey))
            {
                return user.CanvasApiKey;
            }
            
            throw new Exception("User not found or API key not set");
        }

        public async Task<List<ApplicationUser>> GetAllOptedInForEmailsAsync()
        {
            return await _dbContext.Users
                .Where(u => !u.IsOptedOutForEmails && u.Email != null)
                .ToListAsync();
        }

        public async Task SetEmailOptOut(string userId, bool optOut)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user != null)
            {
                user.SetEmailOptOut(optOut);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}