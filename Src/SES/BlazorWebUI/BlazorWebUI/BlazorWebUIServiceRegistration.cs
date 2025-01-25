using BlazorWebUI.Components.Account;
using Domain.Entities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BlazorWebUI;

public static class BlazorWebUIServiceRegistration
{
    public static IServiceCollection AddBlazorWebUIServiceRegistration(this IServiceCollection services)
    {
        services.AddScoped<IdentityUserAccessor>();
        services.AddScoped<IdentityRedirectManager>();
        services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();
        services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

        return services;
    }
}