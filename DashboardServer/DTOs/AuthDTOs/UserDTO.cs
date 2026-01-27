namespace DashboardServer.DTOs.AuthDTOs
{
    public class UserDTO
    {
        public required string Id { get; set; }
        public required string Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool IsApiKeySet { get; set; }
        public DateTime? LastUpdatedData { get; set; }
    }
}