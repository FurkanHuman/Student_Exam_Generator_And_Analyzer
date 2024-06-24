using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Services.AuthService;
using Microsoft.AspNetCore.Http;

namespace BlazorWebUI;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IAuthService _authService;

    public CustomAuthenticationStateProvider(IAuthService authService)
    {
        _authService = authService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string? token = await _authService.GetTokenValueToCookie("accessToken");
        if (string.IsNullOrEmpty(token))
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        JwtSecurityToken jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

        IEnumerable<Claim> claims = jwt.Claims.Where(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");

        ClaimsPrincipal user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Bearer"));

        await _authService.AddUserToAuthPipeline(user);

        return new AuthenticationState(user);
    }

    public void NotifyAuthState() => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

    public void NotifyUserLogout()
    {
        _authService.Logout();
        var identity = new ClaimsIdentity();
        var user = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }
}