namespace DashboardServer.DTOs
{
    public class AuthResultDTO
    {
        public required string Token { get; set; }
        public required UserDTO User { get; set; }
    }
}