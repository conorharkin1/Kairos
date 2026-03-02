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
    }
}