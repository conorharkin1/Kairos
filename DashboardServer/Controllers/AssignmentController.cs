using AutoMapper;
using DashboardServer.DTOs.ModelDTOs.Requests;
using DashboardServer.DTOs.ModelDTOs.Responses;
using DashboardServer.Models;
using DashboardServer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DashboardServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AssignmentController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IAssignmentService _assignmentService;

        public AssignmentController(UserManager<ApplicationUser> userManager, IMapper mapper, IAssignmentService assignmentService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _assignmentService = assignmentService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var assignments = await _assignmentService.GetAllAsync();
            return Ok(_mapper.Map<List<AssignmentResponseDTO>>(assignments));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAssignment([FromBody] AssignmentRequestDTO dto)
        {
            string userId = _userManager.GetUserId(HttpContext.User)!;

            var result = await _assignmentService.CreateAsync(userId, dto);

            return Ok(_mapper.Map<AssignmentResponseDTO>(result));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAssignment(int id, [FromBody] AssignmentRequestDTO dto)
        {
            string userId = _userManager.GetUserId(HttpContext.User)!;

            var result = await _assignmentService.UpdateAsync(userId, id, dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            string userId = _userManager.GetUserId(HttpContext.User)!;
            
            await _assignmentService.DeleteAsync(userId, id);
            return NoContent();
        }
    }
}