using Boilerplate.Beta.Core.Application.Models.Entities;
using Boilerplate.Beta.Core.Application.Repositories.Abstractions;
using Boilerplate.Beta.Core.Application.Services.Abstractions;

namespace Boilerplate.Beta.Core.Application.Services
{
    /// <summary>
    /// Entity service implementation
    /// Uses IReadWriteRepository-like abstraction (IEntityReadWriteRepository) and UnitOfWork for transactional consistency.
    /// </summary>
    public class EntityService : IEntityService
    {
        private readonly IEntityReadWriteRepository _entityReadWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EntityService(IEntityReadWriteRepository entityReadWriteRepository, IUnitOfWork unitOfWork)
        {
            _entityReadWriteRepository = entityReadWriteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Entity?> GetByIdAsync(Guid id)
        {
            return await _entityReadWriteRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Entity>> GetAllAsync()
        {
            return await _entityReadWriteRepository.GetAllAsync();
        }

        public async Task<Entity> AddAsync(Entity entity)
        {
            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                await _entityReadWriteRepository.AddAsync(entity);
            });

            return entity;
        }

        public async Task<Entity> UpdateAsync(Entity entity)
        {
            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                await _entityReadWriteRepository.UpdateAsync(entity);
            });

            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                await _entityReadWriteRepository.DeleteAsync(id);
            });
        }
    }
}
