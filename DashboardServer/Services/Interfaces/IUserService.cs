namespace DashboardServer.Services.Interfaces
{
    public interface IUserService
    {
        Task SetApiKey(string apiKey, string userId);
        Task<string> GetEncryptedApiKey(string userId);
        Task SetEmailOptOut(string userId, bool optOut);
    }
}