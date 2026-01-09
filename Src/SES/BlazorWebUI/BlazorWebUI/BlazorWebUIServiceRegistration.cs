using ApexCharts;
using BlazorWebUI.Components.Account;
using BlazorWebUI.Extensions;
using Domain.Entities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
namespace BlazorWebUI;

public static class BlazorWebUIServiceRegistration
{
    public static IServiceCollection AddBlazorWebUIServiceRegistration(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());
        services.AddApexCharts();
        services.AddScoped<IdentityUserAccessor>();
        services.AddScoped<IdentityRedirectManager>();
        services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();
        services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
        services.AddScoped<RoleManager<IdentityRole<Guid>>>();
        services.AddPoliciesFromJson(configuration);

        return services;
    }
}
