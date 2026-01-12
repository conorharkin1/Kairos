using DashboardServer.Models;

namespace DashboardServer.Services.Interfaces
{
    public interface IGenericConstraintService<T> where T : BaseEntity
    {
        Task<T?> GetConstraintsAsync();
        Task<List<T>> GetListConstraintsAsync();
        Task<T> UpdateConstraintsAsync(T entity);
        Task<List<T>> UpdateListConstraintsAsync(List<T> entities);
    }
}