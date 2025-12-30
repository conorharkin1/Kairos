using DashboardServer.Data;
using DashboardServer.Models;
using DashboardServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DashboardServer.Repositories
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AssignmentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Assignment> Create(string userId, Assignment assignment)
        {
            assignment.UserId = userId;
            var createdAssignment = await _dbContext.Assignments.AddAsync(assignment);
            await _dbContext.SaveChangesAsync();
            return createdAssignment.Entity;
        }

        public async Task<Assignment> Update(string userId, int assignmentId, Assignment assignment)
        {
            var existingAssignment = await _dbContext.Assignments.FirstOrDefaultAsync(a => a.Id == assignmentId && a.UserId == userId);
            if (existingAssignment == null)
            {
                throw new KeyNotFoundException("Assignment not found");
            }

            existingAssignment.Name = assignment.Name;
            existingAssignment.DueDate = assignment.DueDate;
            existingAssignment.Description = assignment.Description;
            existingAssignment.HtmlUrl = assignment.HtmlUrl;

            await _dbContext.SaveChangesAsync();
            return existingAssignment;
        }

        public async Task Delete(string userId, int assignmentId)
        {
            var assignment = await _dbContext.Assignments.FindAsync(assignmentId);
            if (assignment != null && assignment.UserId == userId)
            {
                _dbContext.Assignments.Remove(assignment);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Assignment not found");
            }
        }
    }
}