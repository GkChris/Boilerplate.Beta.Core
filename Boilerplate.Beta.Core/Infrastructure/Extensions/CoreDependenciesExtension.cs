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
            // Application Services
            services.AddScoped<IEntityService, EntityService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Repositories (base)
            services.AddScoped(typeof(IReadWriteRepository<>), typeof(ReadWriteRepository<>));
            
            // Application Repositories
            services.AddScoped<IEntityReadWriteRepository, EntityReadWriteRepository>();
        }
    }
}
