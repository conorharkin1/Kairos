using DashboardServer.Exceptions;

namespace DashboardServer.Models.Constraints
{
    public class SchedulerRules : BaseEntity
    {
        private const int SessionLengthStep = 30;
        private const int SessionLengthMin = 30;
        private const int SessionLengthMax = 240;

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

        public SchedulerRules(string userId, int minSessionLength = 30, int maxSessionLength = 120) : base(userId)
        {
            ValidateSessionLengths(minSessionLength, maxSessionLength);

            MinSessionLength = minSessionLength;
            MaxSessionLength = maxSessionLength;
            DeadlineUrgencyWeighting = 0.7;
            CoursePreferenceWeighting = 0.3;
        }

        public void Update(int minSessionLength, int maxSessionLength)
        {
            ValidateSessionLengths(minSessionLength, maxSessionLength);

            MinSessionLength = minSessionLength;
            MaxSessionLength = maxSessionLength;
        }

        private static void ValidateSessionLengths(int minSessionLength, int maxSessionLength)
        {
            ValidateSessionLength(minSessionLength, "Min");
            ValidateSessionLength(maxSessionLength, "Max");

            if (minSessionLength > maxSessionLength)
            {
                throw new ValidationException("Minimum session length cannot be greater than maximum session length");
            }
        }

        private static void ValidateSessionLength(int value, string label)
        {
            if (value < SessionLengthMin || value > SessionLengthMax)
            {
                throw new ValidationException($"{label} session length must be between {SessionLengthMin} and {SessionLengthMax} minutes");
            }

            if (value % SessionLengthStep != 0)
            {
                throw new ValidationException($"{label} session length must be a multiple of {SessionLengthStep} minutes");
            }
        }

        public void UpdateSchedulerPriorities(double deadlineUrgencyWeighting, double coursePreferenceWeighting)
        {
            if (deadlineUrgencyWeighting + coursePreferenceWeighting != 1.0)
            {
                throw new ValidationException("The sum of the weightings must be equal to 1");
            }
            
            DeadlineUrgencyWeighting = deadlineUrgencyWeighting;
            CoursePreferenceWeighting = coursePreferenceWeighting;
        }
    }
}