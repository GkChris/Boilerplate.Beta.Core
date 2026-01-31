using Boilerplate.Beta.Core.Application.Models.Entities;
using Boilerplate.Beta.Core.Application.Repositories.Abstractions;
using Boilerplate.Beta.Core.Application.Services.Abstractions;

namespace Boilerplate.Beta.Core.Application.Services
{
    /// <summary>
    /// Entity service implementation
    /// Extends base service and adds entity-specific business logic
    /// </summary>
    public class EntityService : Service<Entity>, IEntityService
    {
        private readonly IEntityRepository _entityRepository;

        public EntityService(IEntityRepository entityRepository, IUnitOfWork unitOfWork) : base(entityRepository, unitOfWork)
        {
            _entityRepository = entityRepository;
        }
    }
}
