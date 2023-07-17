using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using N5.Core.Mapping;
using N5.Core.Services;
using N5.Core.Shared;
using N5.Infrastructure.Data;
//using N5.Infrastructure.ElasticSearch;
//using N5.Infrastructure.Messaging;
using N5.Shared.Interfaces;

namespace N5.Infrastructure.IoC
{
    public static class DependencyContainer
    {
        public static IServiceCollection RegisterDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            /*services.AddElasticSearch(configuration);
            services.AddKafka();*/

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<,>), typeof(GenericRepository<,>));
            services.AddScoped<IPermissionService, PermissionService>();

            return services;
        }
    }
}