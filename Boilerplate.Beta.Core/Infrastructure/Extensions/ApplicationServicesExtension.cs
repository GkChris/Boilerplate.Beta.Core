using Boilerplate.Beta.Core.Application.Models.Entities;
using Boilerplate.Beta.Core.Application.Repositories.Abstractions;
using Boilerplate.Beta.Core.Application.Services;
using Boilerplate.Beta.Core.Application.Services.Abstractions;
using Boilerplate.Beta.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Boilerplate.Beta.Core.Infrastructure.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            // Services
            services.AddScoped<IEntityService, EntityService>();

            // Repositories
            services.AddScoped<IRepository<Entity>, EntityRepository>();
            
        }
    }
}
