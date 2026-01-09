using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace BlazorWebUI.Extensions;

public static class UnauthorizedHandlerExtension
{
    public static Func<RedirectContext<CookieAuthenticationOptions>, Task> HandleAccessDenied()
    {
        return ctx =>
        {
            string returnUrl = ctx.Request.Path + ctx.Request.QueryString;

            if (!ctx.Request.Path.StartsWithSegments("/Account", StringComparison.OrdinalIgnoreCase))
                ctx.Response.Redirect($"/AccessDenied?ReturnUrl={Uri.EscapeDataString(returnUrl)}");

            else
                ctx.Response.Redirect($"/Account/AccessDenied?ReturnUrl={Uri.EscapeDataString(returnUrl)}");

            return Task.CompletedTask;
        };
    }
}