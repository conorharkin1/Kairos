namespace DashboardServer.Services.Interfaces
{
    public interface ICanvasApiService
    {
        Task UpdateDataFromCanvasAsync(string userId);
    }
}