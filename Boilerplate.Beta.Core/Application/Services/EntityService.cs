using Boilerplate.Beta.Core.Application.Models.Entities;
using Boilerplate.Beta.Core.Application.Repositories.Abstractions;
using Boilerplate.Beta.Core.Application.Services.Abstractions;

namespace Boilerplate.Beta.Core.Application.Services
{
    /// <summary>
    /// Entity service implementation
    /// Uses IRepository-like abstraction (IEntityReadWriteRepository) and UnitOfWork for transactional consistency.
    /// </summary>
    public class EntityService : IEntityService
    {
        private readonly IEntityReadWriteRepository _entityRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EntityService(IEntityReadWriteRepository entityRepository, IUnitOfWork unitOfWork)
        {
            _entityRepository = entityRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Entity?> GetByIdAsync(Guid id)
        {
            return await _entityRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Entity>> GetAllAsync()
        {
            return await _entityRepository.GetAllAsync();
        }

        public async Task<Entity> AddAsync(Entity entity)
        {
            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                await _entityRepository.AddAsync(entity);
            });

            return entity;
        }

        public async Task<Entity> UpdateAsync(Entity entity)
        {
            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                await _entityRepository.UpdateAsync(entity);
            });

            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                await _entityRepository.DeleteAsync(id);
            });
        }
    }
}
