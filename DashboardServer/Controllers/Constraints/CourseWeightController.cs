using AutoMapper;
using DashboardServer.DTOs.ConstraintDTOs;
using DashboardServer.Models.Constraints;
using DashboardServer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DashboardServer.Controllers.Constraints
{
    [ApiController]
    [Route("api/constraint/weight")]
    [Authorize]
    public class CourseWeightController: ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGenericConstraintService<CourseWeight> _service;
        
        public CourseWeightController(IMapper mapper, IGenericConstraintService<CourseWeight> service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        public async Task<List<CourseWeightDTO>> GetCourseWeightsAsync()
        {
            var weights = await _service.GetListConstraintsAsync();
            return _mapper.Map<List<CourseWeightDTO>>(weights);
        }

        [HttpPut("{courseId}")]
        public async Task<CourseWeightDTO> UpdateCourseWeightAsync(int courseId, [FromBody] CourseWeightDTO dto)
        {
            if (dto.CourseId != courseId)
            {
                throw new ArgumentException("Course ID in URL does not match course ID in body.");
            }
            var model = _mapper.Map<CourseWeight>(dto);
            var updatedModel = await _service.UpdateConstraintsAsync(model);
            return _mapper.Map<CourseWeightDTO>(updatedModel);
        }
    }
}