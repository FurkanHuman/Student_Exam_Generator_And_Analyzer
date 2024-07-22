using Application.Services.CookiesService;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BlazorWebUI;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ICookieService _cookieService;

    public CustomAuthenticationStateProvider(ICookieService cookieService)
    {
        _cookieService = cookieService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string? token = await _cookieService.GetTokenValueToCookie("accessToken");
        if (string.IsNullOrEmpty(token))
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        JwtSecurityToken jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

        IEnumerable<Claim> claims = jwt.Claims.Where(c => c.Type == ClaimTypes.Role);

        ClaimsPrincipal user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Bearer"));

        await _cookieService.AddUserToAuthPipeline(user);
        return new AuthenticationState(user);
    }

    public void NotifyAuthState() => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

    public void NotifyUserLogout()
    {
        _cookieService.CookieLogout();
        ClaimsIdentity identity = new ClaimsIdentity();
        ClaimsPrincipal user = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }
}