namespace DashboardServer.Repositories
{
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
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                user.CanvasApiKey = encryptedApiKey;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}