namespace DashboardServer.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task SetApiKey(string userId, string encryptedApiKey);
    }
}