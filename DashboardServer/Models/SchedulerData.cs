using DashboardServer.Exceptions;
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

        public void SetAssignments(List<Assignment> assignments)
        {
            Assignments = assignments;
        }
        public void SetCourses(List<Course> courses)
        {
            Courses = courses;
        }
        public void SetCourseWeights(List<CourseWeight> courseWeights)
        {
            CourseWeights = courseWeights;
        }
        public void SetDailyCapacities(List<DailyCapacity> dailyCapacities)
        {
            if (dailyCapacities.Count != 7)
            {
                throw new ValidationException("There must be exactly 7 daily capacities, one for each day of the week.");
            }

            DailyCapacities = dailyCapacities;
        }
        public void SetRules(SchedulerRules rules)
        {
            Rules = rules;
        }
        public void SetAvoidPeriods(List<AvoidPeriod> avoidPeriods)
        {
            AvoidPeriods = avoidPeriods;
        }
    }
}