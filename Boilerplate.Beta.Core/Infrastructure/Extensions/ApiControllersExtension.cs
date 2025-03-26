using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace Boilerplate.Beta.Core.Infrastructure.Extensions
{
    public static class ApiControllersExtension
    {
        public static void AddApiControllers(this IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        }
    }
}
