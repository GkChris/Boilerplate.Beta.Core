namespace Boilerplate.Beta.Core.Application.Services.Abstractions
{
    /// <summary>
    /// Base interface for generic service operations
    /// Provides standard CRUD operations for any entity type
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public interface IService<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
    }
}
