using Boilerplate.Beta.Core.Application.Repositories.Abstractions;
using Boilerplate.Beta.Core.Application.Services.Abstractions;

namespace Boilerplate.Beta.Core.Application.Services
{
    /// <summary>
    /// Base service class providing generic CRUD operations
    /// Wraps repository layer and adds service-level logic
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public class Service<T> : IService<T> where T : class
    {
        protected readonly IRepository<T> _repository;
        protected readonly IUnitOfWork _unitOfWork;

        public Service(IRepository<T> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            T result = null;
            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                result = await _repository.AddAsync(entity);
            });
            return result;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            T result = null;
            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                result = await _repository.UpdateAsync(entity);
            });
            return result;
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                await _repository.DeleteAsync(id);
            });
        }
    }
}
