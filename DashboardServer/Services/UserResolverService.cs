using DashboardServer.Services.Interfaces;

namespace DashboardServer.Services
{
    public class UserResolverService: IUserResolverService
    {
        private readonly IHttpContextAccessor _httpContext;

        public UserResolverService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor;
        }

        public string? UserId => _httpContext.HttpContext?.User?.FindFirst("uid")?.Value;
    }
}