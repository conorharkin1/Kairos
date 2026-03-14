using DashboardServer.Exceptions;

namespace DashboardServer.Models.Constraints
{
    public class SchedulerRules : BaseEntity
    {
        /// <summary>
        /// Min length of each session 'block' in minutes
        /// </summary>
        public int MinSessionLength { get; private set; }
        /// <summary>
        /// Max length of consecutive sessions in minutes
        /// </summary>
        public int MaxSessionLength { get; private set; }
        /// <summary>
        /// Deadline Urgency Weighting
        /// Used in the Weighted linear Combination formula in the scheduler
        /// Default value 0.7
        /// </summary>
        public double DeadlineUrgencyWeighting { get; private set; }
        /// <summary>
        /// Course Preference Weighting
        /// Used in the Weighted linear Combination formula in the scheduler
        /// Default value 0.3
        /// </summary>
        public double CoursePreferenceWeighting { get; private set; }

        protected SchedulerRules() { }

        public SchedulerRules(string userId, int minSessionLength = 30, int maxSessionLength = 180) : base(userId)
        {
            if (minSessionLength <= 0)
            {
                throw new ValidationException("Min session length must be a positive integer");
            }

            if (maxSessionLength <= 0)
            {
                throw new ValidationException("Max session length must be a positive integer");
            }

            if (minSessionLength > maxSessionLength)
            {
                throw new ValidationException("Minimum session length cannot be greater than maximum session length");
            }

            MinSessionLength = minSessionLength;
            MaxSessionLength = maxSessionLength;
            DeadlineUrgencyWeighting = 0.7;
            CoursePreferenceWeighting = 0.3;
        }

        public void Update(int minSessionLength, int maxSessionLength)
        {
            if (minSessionLength <= 0)
            {
                throw new ValidationException("Min session length must be a positive integer");
            }

            if (maxSessionLength <= 0)
            {
                throw new ValidationException("Max session length must be a positive integer");
            }

            if (minSessionLength > maxSessionLength)
            {
                throw new ValidationException("Minimum session length cannot be greater than maximum session length");
            }

            MinSessionLength = minSessionLength;
            MaxSessionLength = maxSessionLength;
        }

        public void UpdateSchedulerPriorities(double deadlineUrgencyWeighting, double coursePreferenceWeighting)
        {
            DeadlineUrgencyWeighting = deadlineUrgencyWeighting;
            CoursePreferenceWeighting = coursePreferenceWeighting;
        }
    }
}