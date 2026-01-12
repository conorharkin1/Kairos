using DashboardServer.Data;
using DashboardServer.Models;
using DashboardServer.Repositories.Interfaces;
using DashboardServer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DashboardServer.Repositories
{
    public class GenericConstraintRepository<T> : IGenericConstraintRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserResolverService _userResolverService;
        public string? CurrentUserId => _userResolverService.UserId;

        public GenericConstraintRepository(ApplicationDbContext dbContext, IUserResolverService userResolverService)
        {
            _dbContext = dbContext;
            _userResolverService = userResolverService;
        }
        public async Task<T?> GetConstraintsAsync()
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetListConstraintsAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> UpdateConstraintsAsync(T entity)
        {
            var existingEntity = await _dbContext.Set<T>().FirstOrDefaultAsync();
            if (existingEntity == null)
            {
                entity.UserId = CurrentUserId!;
                _dbContext.Set<T>().Add(entity);
            }
            else
            {
                _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
            }

            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<List<T>> UpdateListConstraintsAsync(List<T> entities)
        {
            var existingConstraints = await _dbContext.Set<T>().ToListAsync();
            var entitiesToAdd = new List<T>();

            foreach(var entity in entities)
            {
                entity.UserId = CurrentUserId!;
                var existingEntity = existingConstraints.FirstOrDefault(e => e.Id == entity.Id);

                if (existingEntity == null)
                {
                    entitiesToAdd.Add(entity);
                }
                else
                {
                    _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
                }
            }
            
            _dbContext.Set<T>().AddRange(entitiesToAdd);
            
            await _dbContext.SaveChangesAsync();
            return entities;
        }
    }
}