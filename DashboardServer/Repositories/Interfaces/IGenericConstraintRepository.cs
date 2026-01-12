using DashboardServer.Models;

namespace DashboardServer.Repositories.Interfaces
{
    public interface IGenericConstraintRepository<T> where T : BaseEntity
    {
        public Task<T?> GetConstraintsAsync();
        public Task<List<T>> GetListConstraintsAsync();
        public Task<T> UpdateConstraintsAsync(T entity);
        public Task<List<T>> UpdateListConstraintsAsync(List<T> entities);
    }
}