using App;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES_GUI;

public static class SesGUIServiceRegistration
{
    public static IServiceCollection AddSesGUIServiceRegistration(this IServiceCollection services)
    {

        services.AddScoped<SES_Main>();
        services.AddScoped<Analysis_Full>();
        return services;
    }
}