using DashboardServer.Models.Constraints;

namespace DashboardServer.Models
{
    public class SchedulerData()
    {
        public List<Assignment>? Assignments { get; set; }
        public List<Course>? Courses { get; set; }
        public List<CourseWeight>? CourseWeights { get; set; }
        public List<DailyCapacity>? DailyCapacities { get; set; }
        public SchedulerRules? Rules { get; set; }
    }
}