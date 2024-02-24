using Microsoft.Extensions.DependencyInjection;
using Persistence;
using App;

namespace SES_GUI;

public static class SesGUIServiceRegistration
{
    public static IServiceCollection AddSesGUIServiceRegistration(this IServiceCollection services)
    {
        services.AddAppServiceRegistration();
        services.AddPersistenceServices();

        services.AddTransient<SES_Main>();
        services.AddTransient<Analysis_Full>();
        services.AddTransient<Student_Add>();
        return services;
    }
}