using DashboardServer.Exceptions;

namespace DashboardServer.Models.Constraints
{
    public class CourseWeight : BaseEntity
    {
        public int CourseId { get; private set; }
        public Course Course { get; private set; } = default!;
        /// <summary>
        /// Weight of the course as a percentage
        /// </summary>
        public int Weight { get; private set; }

        protected CourseWeight() { }

        public CourseWeight(string userId, int courseId, int weight) : base(userId)
        {
            if (courseId <= 0)
            {
                throw new ValidationException("Valid Course Id is required");
            }

            if (weight < 0 || weight > 100)
            {
                throw new ValidationException("Weight must be between 0 and 100");
            }

            CourseId = courseId;
            Weight = weight;
        }

        public void Update(int weight)
        {
            if (weight < 0 || weight > 100)
            {
                throw new ValidationException("Weight must be between 0 and 100");
            }

            Weight = weight;
        }
    }
}