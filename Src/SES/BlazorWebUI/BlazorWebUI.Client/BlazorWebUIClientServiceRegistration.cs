using Identity.Client;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorWebUI.Client;

public static class BlazorWebUIClientServiceRegistration
{
    public static IServiceCollection AddBlazorWebUIClientServiceRegistration(this IServiceCollection services)
    {
        services.AddAuthorizationCore();
        services.AddCascadingAuthenticationState();
        services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

        return services;
    }
}