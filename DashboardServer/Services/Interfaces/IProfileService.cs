namespace DashboardServer.Services.Interfaces
{
    public interface IProfileService
    {
        Task SetApiKey(string userId, string apiKey);
    }
}