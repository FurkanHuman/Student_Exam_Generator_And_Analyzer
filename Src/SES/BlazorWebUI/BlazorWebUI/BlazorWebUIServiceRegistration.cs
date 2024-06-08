using Application;
using Persistence;

namespace BlazorWebUI;

public static class BlazorWebUIServiceRegistration
{
    public static IServiceCollection AddBlazorWebUIServiceRegistration(this IServiceCollection services)
    {
        services.AddAppicationServiceRegistration();
        services.AddPersistenceServiceRegistration();
        return services;
    }
}