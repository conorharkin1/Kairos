using DashboardServer.Services.Interfaces;
using Hangfire;

namespace DashboardServer.Services
{
    public class SchedulerService : ISchedulerService
    {
        private readonly IUserResolverService _userResolverService;
        public SchedulerService(IUserResolverService userResolverService)
        {
            _userResolverService = userResolverService;
        }
        public void RunScheduler()
        {
            string userId = _userResolverService.UserId!;
            BackgroundJob.Enqueue(() => BackgroundRunScheduler(userId));
        }

        public void BackgroundRunScheduler(string userId)
        {
            Console.WriteLine("THE BACKGROUND JOB IS RUNNING");
        }
    }
}