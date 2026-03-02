using DashboardServer.Exceptions;

namespace DashboardServer.Models.Constraints
{
    public class DailyCapacity : BaseEntity
    {
        /// <summary>
        /// 0 = Sunday 6 = Saturday
        /// </summary>
        public int DayIndex { get; private set; }
        /// <summary>
        /// Maximum available hours for this day
        /// </summary>
        public int MaxHours { get; private set; }

        protected DailyCapacity() { }

        public DailyCapacity(string userId, int dayIndex, int maxHours) : base(userId)
        {
            if (dayIndex < 0 || dayIndex > 6)
            {
                throw new ValidationException("Day index must be between 0 (Sunday) and 6 (Saturday)");
            }

            if (maxHours < 0)
            {
                throw new ValidationException("Max hours cannot be negative");
            }

            DayIndex = dayIndex;
            MaxHours = maxHours;
        }

        public void Update(int maxHours)
        {
            if (maxHours < 0)
            {
                throw new ValidationException("Max hours cannot be negative");
            }

            MaxHours = maxHours;
        }
    }
}