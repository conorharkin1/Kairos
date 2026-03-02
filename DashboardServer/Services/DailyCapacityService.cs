using AutoMapper;
using DashboardServer.DTOs.ConstraintDTOs.Requests;
using DashboardServer.DTOs.ConstraintDTOs.Responses;
using DashboardServer.Exceptions;
using DashboardServer.Models.Constraints;
using DashboardServer.Repositories.Interfaces;
using DashboardServer.Services.Interfaces;

namespace DashboardServer.Services
{
    public class DailyCapacityService : IDailyCapacityService
    {
        private readonly IDailyCapacityRepository _repo;
        private readonly IMapper _mapper;
        private readonly IUserResolverService _userResolverService;

        public DailyCapacityService(IDailyCapacityRepository repository, IMapper mapper, IUserResolverService userResolverService)
        {
            _repo = repository;
            _mapper = mapper;
            _userResolverService = userResolverService;
        }

        private string CurrentUserId => _userResolverService.UserId!;

        public async Task<List<DailyCapacityResponseDTO>> GetAllAsync()
        {
            var capacities = await _repo.GetAllAsync();
            if (capacities.Count == 0)
            {
                capacities = CreateDefaults();
                await _repo.CreateBulkAsync(capacities);
            }

            return _mapper.Map<List<DailyCapacityResponseDTO>>(capacities);
        }

        public async Task<List<DailyCapacityResponseDTO>> UpdateAsync(List<DailyCapacityRequestDTO> dtos)
        {
            if (dtos.Count != 7)
            {
                throw new ValidationException("There must be exactly 7 daily capacities, one for each day of the week.");
            }

            if (dtos.Any(d => d.DayIndex < 0 || d.DayIndex > 6))
            {
                throw new ValidationException("Day indices must be between 0 and 6.");
            }

            var existing = await _repo.GetAllAsync();
            var existingByDay = existing.ToDictionary(c => c.DayIndex);
            var toCreate = new List<DailyCapacity>();

            foreach (var dto in dtos)
            {
                if (existingByDay.TryGetValue(dto.DayIndex, out var capacity))
                {
                    capacity.Update(dto.MaxHours);
                }
                else
                {
                    toCreate.Add(new DailyCapacity(CurrentUserId, dto.DayIndex, dto.MaxHours));
                }
            }

            if (toCreate.Count > 0)
            {
                await _repo.CreateBulkAsync(toCreate);
            }

            await _repo.Save();

            var result = await _repo.GetAllAsync();
            return _mapper.Map<List<DailyCapacityResponseDTO>>(result);
        }

        private List<DailyCapacity> CreateDefaults()
        {
            return Enumerable.Range(0, 7)
                .Select(index => new DailyCapacity(CurrentUserId, index, 0))
                .ToList();
        }
    }
}