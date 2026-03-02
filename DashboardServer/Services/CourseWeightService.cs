using AutoMapper;
using DashboardServer.DTOs.ConstraintDTOs.Requests;
using DashboardServer.DTOs.ConstraintDTOs.Responses;
using DashboardServer.Exceptions;
using DashboardServer.Models.Constraints;
using DashboardServer.Repositories.Interfaces;
using DashboardServer.Services.Interfaces;

namespace DashboardServer.Services
{
    public class CourseWeightService : ICourseWeightService
    {
        private readonly ICourseWeightRepository _repo;
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;
        private readonly IUserResolverService _userResolverService;

        public CourseWeightService(ICourseWeightRepository repository, ICourseRepository courseRepository, IMapper mapper, IUserResolverService userResolverService)
        {
            _repo = repository;
            _courseRepository = courseRepository;
            _mapper = mapper;
            _userResolverService = userResolverService;
        }

        private string CurrentUserId => _userResolverService.UserId!;

        public async Task<List<CourseWeightResponseDTO>> GetAllAsync()
        {
            var courseIds = await _courseRepository.GetCourseIdsAsync();
            var weights = await _repo.GetAllAsync();

            if (weights.Count == 0)
            {
                weights = CreateDefaults(courseIds);
                await _repo.CreateBulkAsync(weights);
            }
            else if (weights.Count != courseIds.Count)
            {
                var missingIds = courseIds.Where(ci => !weights.Any(w => w.CourseId == ci)).ToList();
                var newDefaults = CreateDefaults(missingIds);
                await _repo.CreateBulkAsync(newDefaults);
                weights.AddRange(newDefaults);
            }

            return _mapper.Map<List<CourseWeightResponseDTO>>(weights);
        }

        public async Task<List<CourseWeightResponseDTO>> UpdateAsync(List<CourseWeightRequestDTO> dtos)
        {
            if (dtos.Count == 0)
            {
                return new List<CourseWeightResponseDTO>();
            }

            var courseIds = await _courseRepository.GetCourseIdsAsync();
            if (dtos.Any(d => !courseIds.Contains(d.CourseId)))
            {
                throw new ValidationException("Attempting to update weight preference of a Course that doesn't exist");
            }

            var existing = await _repo.GetAllAsync();
            var existingByCourse = existing.ToDictionary(w => w.CourseId);
            var toCreate = new List<CourseWeight>();

            foreach (var dto in dtos)
            {
                if (existingByCourse.TryGetValue(dto.CourseId, out var weight))
                {
                    weight.Update(dto.Weight);
                }
                else
                {
                    toCreate.Add(new CourseWeight(CurrentUserId, dto.CourseId, dto.Weight));
                }
            }

            if (toCreate.Count > 0)
            {
                await _repo.CreateBulkAsync(toCreate);
            }

            await _repo.Save();

            var result = await _repo.GetAllAsync();
            return _mapper.Map<List<CourseWeightResponseDTO>>(result);
        }

        private List<CourseWeight> CreateDefaults(List<int> courseIds)
        {
            return courseIds.Select(id => new CourseWeight(CurrentUserId, id, 0)).ToList();
        }
    }
}