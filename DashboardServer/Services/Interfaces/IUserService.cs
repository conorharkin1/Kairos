namespace DashboardServer.Services.Interfaces
{
    public interface IUserService
    {
        Task SetApiKey(string apiKey, Guid key, string userId);
        Task<string> GetEncryptedApiKey(string userId);
    }
}