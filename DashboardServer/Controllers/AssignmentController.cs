using AutoMapper;
using DashboardServer.DTOs.ModelDTOs;
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
            string userId = _userManager.GetUserId(HttpContext.User)!;

            var assignments = await _assignmentService.GetAll(userId);
            return Ok(_mapper.Map<List<AssignmentDTO>>(assignments));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAssignment([FromBody] AssignmentDTO assignmentDto)
        {
            string userId = _userManager.GetUserId(HttpContext.User)!;

            var assignment = _mapper.Map<Assignment>(assignmentDto);
            var createdAssignment = await _assignmentService.Create(userId, assignment);
            return Ok(_mapper.Map<AssignmentDTO>(createdAssignment));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAssignment(int id, [FromBody] AssignmentDTO assignmentDto)
        {
            string userId = _userManager.GetUserId(HttpContext.User)!;

            var assignment = _mapper.Map<Assignment>(assignmentDto);
            var updatedAssignment = await _assignmentService.Update(userId, id, assignment);
            return Ok(_mapper.Map<AssignmentDTO>(updatedAssignment));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            string userId = _userManager.GetUserId(HttpContext.User)!;
            
            await _assignmentService.Delete(userId, id);
            return NoContent();
        }
    }
}