using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorWebUI;

public static class BlazorWebUIServiceRegistration
{
    public static IServiceCollection AddBlazorWebUIServiceRegistration(this IServiceCollection services)
    {

        services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
        services.AddHttpContextAccessor();
        return services;
    }
}