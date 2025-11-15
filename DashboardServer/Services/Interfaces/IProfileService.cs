namespace DashboardServer.Services.Interfaces
{
    public interface IProfileService
    {
        Task SetApiKey(string apiKey, Guid key, string userId);
    }
}