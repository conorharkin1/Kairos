namespace DashboardServer.DTOs
{
    public class UserDTO
    {
        public required string Id { get; set; }
        public required string Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool IsApiKeySet { get; set; }
    }
}