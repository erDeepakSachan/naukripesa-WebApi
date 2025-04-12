using App.Core;
using Microsoft.Extensions.DependencyInjection;

namespace App.Service;

public class DI : IServiceRegistrar
{
    public void RegisterServices(IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
        
        // Get all types implementing GenericService<T> dynamically
        var serviceTypes = typeof(DI).Assembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.BaseType != null &&
                        t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(GenericService<>));

        foreach (var serviceType in serviceTypes)
        {
            services.AddScoped(serviceType);
        }

        services.AddScoped<CommonService>();
    }
}