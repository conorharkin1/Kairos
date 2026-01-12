using DashboardServer.Models;
using DashboardServer.Repositories.Interfaces;
using DashboardServer.Services.Interfaces;

namespace DashboardServer.Services
{
    public class GenericConstraintService<T> : IGenericConstraintService<T> where T : BaseEntity 
    {
        private readonly IGenericConstraintRepository<T> _repository;
        public GenericConstraintService(IGenericConstraintRepository<T> repository)
        {
            _repository = repository;
        }
        public async Task<T?> GetConstraintsAsync()
        {
            return await _repository.GetConstraintsAsync();
        }

        public async Task<List<T>> GetListConstraintsAsync()
        {
            return await _repository.GetListConstraintsAsync();
        }

        public async Task<T> UpdateConstraintsAsync(T entity)
        {
            return await _repository.UpdateConstraintsAsync(entity);
        }

        public async Task<List<T>> UpdateListConstraintsAsync(List<T> entities)
        {
            return await _repository.UpdateListConstraintsAsync(entities);
        }
    }
}