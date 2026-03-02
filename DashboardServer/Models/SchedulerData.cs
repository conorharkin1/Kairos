using DashboardServer.Models.Constraints;

namespace DashboardServer.Models
{
    public class SchedulerData
    {
        public List<Assignment> Assignments { get; private set; }
        public List<Course> Courses { get; private set; }
        public List<CourseWeight> CourseWeights { get; private set; }
        public List<DailyCapacity> DailyCapacities { get; private set; }
        public SchedulerRules? Rules { get; private set; }
        public List<AvoidPeriod> AvoidPeriods { get; private set; }

        public SchedulerData(
            List<Assignment> assignments,
            List<Course> courses,
            List<CourseWeight> courseWeights,
            List<DailyCapacity> dailyCapacities,
            SchedulerRules? rules,
            List<AvoidPeriod> avoidPeriods)
        {
            Assignments = assignments;
            Courses = courses;
            CourseWeights = courseWeights;
            DailyCapacities = dailyCapacities;
            Rules = rules;
            AvoidPeriods = avoidPeriods;
        }
    }
}