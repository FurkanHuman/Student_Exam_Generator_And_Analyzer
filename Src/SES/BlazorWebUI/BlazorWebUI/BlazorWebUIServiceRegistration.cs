using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorWebUI;

public static class BlazorWebUIServiceRegistration
{
    public static IServiceCollection AddBlazorWebUIServiceRegistration(this IServiceCollection services)
    {

        services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
        services.AddHttpContextAccessor();
        services.AddAntiforgery(options => options.HeaderName = "X-CSRF-TOKEN");

        return services;
    }
}