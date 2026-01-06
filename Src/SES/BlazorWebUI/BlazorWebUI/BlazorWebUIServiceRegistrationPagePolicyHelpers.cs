using System.Security.Claims;

namespace BlazorWebUI;

internal static class BlazorWebUIServiceRegistrationPagePolicyHelpers
{

    public static IServiceCollection AddPoliciesFromJson(this IServiceCollection services, IConfiguration configuration)
    {
        Dictionary<string, string>? policies = configuration.GetSection("Policies").Get<Dictionary<string, string>>();

        services.AddAuthorization(cfg =>
        {
            foreach (KeyValuePair<string, string> policy in policies!)
                cfg.AddPolicy(policy.Key, policyBuilder => policyBuilder.RequireAssertion(context => EvaluateRoles(context.User, policy.Value)));
        });

        return services;
    }

    private static bool EvaluateRoles(ClaimsPrincipal user, string expression)
    {
        IEnumerable<string> orGroups = expression.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(g => g.Trim());

        foreach (string group in orGroups)
        {
            if (group.Contains('&'))
            {
                IEnumerable<string> andRoles = group.Split('&', StringSplitOptions.RemoveEmptyEntries)
                    .Select(r => r.Trim());

                if (andRoles.All(role => user.IsInRole(role)))
                    return true;
            }

            else
            {
                if (user.IsInRole(group))
                    return true;
            }
        }

        return false;
    }
}