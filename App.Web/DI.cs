using App.Core;
using App.Web.Fx;

namespace App.Web;

public class DI: IServiceRegistrar
{
    public void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<NeoAuthorizeAttribute>();
    }
}