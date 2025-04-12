using App.Core;
using Microsoft.Extensions.DependencyInjection;

namespace App.Repository;

public class DI : IServiceRegistrar
{
    public void RegisterServices(IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        
        // Get all types implementing Repository<T> dynamically
        var repositoryTypes = typeof(DI).Assembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.BaseType != null &&
                        t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(GenericRepository<>));

        foreach (var repoType in repositoryTypes)
        {
            services.AddScoped(repoType);
        }

        services.AddScoped<CommonRepository>();
    }
}