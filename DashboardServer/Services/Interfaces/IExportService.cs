namespace DashboardServer.Services.Interfaces
{
    public interface IExportService
    {
        public Task<byte[]> GetScheduleAsIcsAsync();
    }
}