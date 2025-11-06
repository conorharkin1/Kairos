using Microsoft.AspNetCore.Mvc;

namespace DashboardServer.Controllers
{
    [ApiController]
    [Route("api/")]
    public class ProfileController : ControllerBase
    {
        [HttpPost("profile/key")]
        public IActionResult SetApiKey([FromBody] string key)
        {
            // Get the current user
            // Encrypt the key
            // Set user.key = encrytptedKey
            // Send encrypted key to repo layer
            return Ok();
        }
    }

    public class ApiKeyRequest
    {
        public string Key { get; set; }
    }
}