using System.Diagnostics;
using DashboardServer.Data;
using DashboardServer.Exceptions;
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

        public async Task<List<Assignment>> GetAllAsync()
        {
            return await _dbContext.Assignments.AsNoTracking().ToListAsync();
        }

        public async Task<List<Assignment>> GetAllCurrentAsync()
        {
            return await _dbContext.Assignments.Where(a => a.DueDate >= DateTime.UtcNow).ToListAsync();
        }

        public async Task<List<Assignment>> GetAllUpcomingUnsubmittedAsync()
        {
            return await _dbContext.Assignments
                .IgnoreQueryFilters()
                .Include(a => a.Course)
                .Where(a => a.DueDate > DateTime.UtcNow && !a.IsComplete)
                .OrderBy(a => a.DueDate)
                .ToListAsync();
        }

        public async Task<Assignment?> GetByIdAsync(int id)
        {
            return await _dbContext.Assignments.FindAsync(id);
        }

        public async Task<Assignment> CreateAsync(Assignment assignment)
        {
            var createdAssignment = await _dbContext.Assignments.AddAsync(assignment);
            await _dbContext.SaveChangesAsync();
            return createdAssignment.Entity;
        }

        public async Task CreateBulkAsync(List<Assignment> assignments)
        {
            await _dbContext.Assignments.AddRangeAsync(assignments);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Assignment assignment)
        {
            _dbContext.Assignments.Remove(assignment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}