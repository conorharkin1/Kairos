using DashboardServer.Exceptions;
using DashboardServer.Models;
using DashboardServer.Models.Constraints;
using DashboardServer.Repositories.Interfaces;
using DashboardServer.Services.Interfaces;

namespace DashboardServer.Services
{
    public class CourseWeightService: ICourseWeightService
    {
        private readonly ICourseWeightRepository _repository;
        private readonly ICourseRepository _courseRepository;
        private readonly IUserResolverService _userResolverService;

        public CourseWeightService(ICourseWeightRepository repository, ICourseRepository courseRepository, IUserResolverService userResolverService)
        {
            _repository = repository;
            _courseRepository = courseRepository;
            _userResolverService = userResolverService;
        }

        private string? CURRENT_USER_ID => _userResolverService.UserId;

        public async Task<List<CourseWeight>> GetCourseWeightsAsync()
        {
            var courseIds = await _courseRepository.GetCourseIdsAsync();
            var constraints = await _repository.GetCourseWeightsAsync();
            if (constraints.Count == 0)
            {
                return await CreateDefaultCourseWeights(courseIds);
            }

            if (constraints.Count != courseIds.Count)
            {
                var missingIds = courseIds.Where(ci => !constraints.Any(c => c.CourseId == ci)).ToList();
                constraints.AddRange(await CreateDefaultCourseWeights(missingIds));
            }

            return constraints;
        }

        public async Task<List<CourseWeight>> UpdateCourseWeightsAsync(List<CourseWeight> weights)
        {
            if (weights.Count == 0)
            {
                return weights;
            }
            
            var courseIds = await _courseRepository.GetCourseIdsAsync();
            if (weights.All(w => !courseIds.Contains(w.CourseId)))
            {
                throw new ValidationException("Attempting to update weight preference of a Course that doesn't exist");
            }

            if (weights.Any(w => w.Weight < 0 || w.Weight > 100))
            {
                throw new ValidationException("Course weights must be between 0 and 100.");
            }

            var updated = await _repository.UpdateCourseWeights(weights, CURRENT_USER_ID!);
            return updated;
        }

        private async Task<List<CourseWeight>> CreateDefaultCourseWeights(List<int> courseIds)
        {
            return courseIds.Select(id => new CourseWeight
            {
               CourseId = id,
               Weight = 0
            }).ToList();

        }
    }
}