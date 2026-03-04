namespace DashboardServer.Repositories.Interfaces
{
    using DashboardServer.Models;

    public interface IUserRepository
    {
        Task SetApiKey(string userId, string encryptedApiKey);
        Task<string> GetEncryptedApiKey(string userId);
        Task<List<ApplicationUser>> GetAllOptedInForEmailsAsync();
        Task SetEmailOptOut(string userId, bool optOut);
    }
}