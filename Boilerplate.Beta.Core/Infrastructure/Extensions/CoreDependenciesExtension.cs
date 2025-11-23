using Boilerplate.Beta.Core.Application.Repositories;
using Boilerplate.Beta.Core.Application.Repositories.Abstractions;
using Boilerplate.Beta.Core.Application.Services;
using Boilerplate.Beta.Core.Application.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Boilerplate.Beta.Core.Infrastructure.Extensions
{
    public static class CoreDependenciesExtension
    {
        public static void AddCoreDependencies(this IServiceCollection services)
        {
            // Generic Services (base)
            services.AddScoped(typeof(IService<>), typeof(Service<>));

            // Specific Services
            services.AddScoped<IEntityService, EntityService>();

            // Generic Repositories (base)
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            
            // Specific Repositories
            services.AddScoped<IEntityRepository, EntityRepository>();
        }
    }
}
