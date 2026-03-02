using AutoMapper;
using DashboardServer.DTOs.ConstraintDTOs;
using DashboardServer.DTOs.ConstraintDTOs.Requests;
using DashboardServer.DTOs.ConstraintDTOs.Responses;
using DashboardServer.Exceptions;
using DashboardServer.Models.Constraints;
using DashboardServer.Repositories.Interfaces;
using DashboardServer.Services.Interfaces;

namespace DashboardServer.Services
{
    public class AvoidPeriodService : IAvoidPeriodService
    {
        private readonly IAvoidPeriodRepository _repo;
        private readonly IMapper _mapper;
        private readonly IUserResolverService _userResolverService;

        public AvoidPeriodService(IAvoidPeriodRepository repository, IMapper mapper, IUserResolverService userResolverService)
        {
            _repo = repository;
            _mapper = mapper;
            _userResolverService = userResolverService;
        }

        private string CurrentUserId => _userResolverService.UserId!;

        public async Task<List<AvoidPeriodResponseDTO>> GetAllAsync()
        {
            var periods = await _repo.GetAllAsync();
            if (periods.Count == 0)
            {
                periods = CreateDefaults();
                await _repo.CreateBulkAsync(periods);
            }

            return _mapper.Map<List<AvoidPeriodResponseDTO>>(periods);
        }

        public async Task<List<AvoidPeriodResponseDTO>> UpdateAsync(List<AvoidPeriodRequestDTO> dtos)
        {
            if (dtos.Count != 7)
            {
                throw new ValidationException("There must be exactly 7 avoid period constraints, one for each day of the week.");
            }

            if (dtos.Any(d => d.DayIndex < 0 || d.DayIndex > 6))
            {
                throw new ValidationException("Day indices must be between 0 and 6.");
            }

            var existing = await _repo.GetAllAsync();
            var existingByDay = existing.ToDictionary(p => p.DayIndex);
            var toCreate = new List<AvoidPeriod>();

            foreach (var dto in dtos)
            {
                var timeBlocks = dto.AvoidTimeBlocks?
                    .Select(tb => new AvoidTimeBlock(tb.StartTime, tb.EndTime))
                    .ToList() ?? new List<AvoidTimeBlock>();

                if (existingByDay.TryGetValue(dto.DayIndex, out var period))
                {
                    period.UpdateTimeBlocks(timeBlocks);
                }
                else
                {
                    toCreate.Add(new AvoidPeriod(CurrentUserId, dto.DayIndex, timeBlocks));
                }
            }

            if (toCreate.Count > 0)
            {
                await _repo.CreateBulkAsync(toCreate);
            }

            await _repo.Save();

            var result = await _repo.GetAllAsync();
            return _mapper.Map<List<AvoidPeriodResponseDTO>>(result);
        }

        private List<AvoidPeriod> CreateDefaults()
        {
            return Enumerable.Range(0, 7)
                .Select(index => new AvoidPeriod(CurrentUserId, index))
                .ToList();
        }
    }
}