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
        string[] orGroups = expression.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        return orGroups.Any(group => IsGroupSatisfied(user, group));
    }

    private static bool IsGroupSatisfied(ClaimsPrincipal user, string group)
    {
        string[] andRoles = group.Split('&', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        return andRoles.All(role => IsRoleConditionMet(user, role));
    }

    private static bool IsRoleConditionMet(ClaimsPrincipal user, string role)
    {
        if (role.StartsWith('!'))
        {
            string excludedRole = role[1..];
            return !user.IsInRole(excludedRole);
        }

        return user.IsInRole(role);
    }

}