using Microsoft.Extensions.DependencyInjection;

namespace SES_GUI;

public static class SesGUIServiceRegistration
{
    public static IServiceCollection AddSesGUIServiceRegistration(this IServiceCollection services)
    {

        services.AddTransient<SES_Main>();
        services.AddTransient<Analysis_Full>();
        services.AddTransient<Student_Add>();
        return services;
    }
}