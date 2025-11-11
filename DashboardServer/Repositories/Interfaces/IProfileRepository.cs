namespace DashboardServer.Repositories.Interfaces
{
    public interface IProfileRepository
    {
        Task SetApiKey(string userId, string encryptedApiKey);
    }
}