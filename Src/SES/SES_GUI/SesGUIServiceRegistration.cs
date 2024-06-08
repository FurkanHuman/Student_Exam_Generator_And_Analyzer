using Application;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using SES_GUI.UI;

namespace SES_GUI;

public static class SesGUIServiceRegistration
{
    public static IServiceCollection AddSesGUIServiceRegistration(this IServiceCollection services)
    {
        services.AddAppicationServiceRegistration();
        services.AddPersistenceServiceRegistration();

        services.AddTransient<SES_Main>();
        services.AddTransient<Analysis_Full>();
        services.AddTransient<Student_Add>();
        services.AddTransient<DbConnectionBuilder>();
        services.AddTransient <SchoolAdd>();
        return services;
    }
}