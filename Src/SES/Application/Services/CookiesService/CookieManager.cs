using Domain.Entities;
using Microsoft.AspNetCore.Http;
using NArchitecture.Core.Security.JWT;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.Services.CookiesService;

public class CookieManager : ICookieService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly CookieOptions _cookieOptions = new() { HttpOnly = true, Secure = true };

    private const string AccessToken = "accessToken";
    private const string RefreshToken = "refreshToken";

    public CookieManager(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

    public Task SetAccessTokenToCookies(AccessToken accessToken)
    {
        _cookieOptions.Expires = accessToken.ExpirationDate;
        _httpContextAccessor.HttpContext.Response.Cookies.Append(nameof(accessToken), accessToken.Token, _cookieOptions);
        return Task.CompletedTask;
    }

    public Task SetRefreshTokenToCookies(RefreshToken? refreshToken)
    {
        if (refreshToken == null)
            return Task.CompletedTask;
        _cookieOptions.Expires = refreshToken.ExpirationDate;
        _httpContextAccessor.HttpContext.Response.Cookies.Append(nameof(refreshToken), refreshToken.Token);
        return Task.CompletedTask;
    }

    public Task<string> GetTokenValueToCookie(string val)
    {
        return Task.FromResult<string>(_httpContextAccessor.HttpContext.Request.Cookies[val]);
    }

    public Task<string> GetIpV4AndIpV6Client()
    {
        System.Net.IPAddress? ips = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;
        return Task.FromResult<string>($"IPv4: {ips.MapToIPv4()}, IPv6: {ips.MapToIPv6()}");
    }

    public Task CookieLogout()
    {
        _httpContextAccessor.HttpContext.Response.Cookies.Delete(RefreshToken);
        _httpContextAccessor.HttpContext.Response.Cookies.Delete(AccessToken);

        return Task.CompletedTask;
    }

    public Task AddUserToAuthPipeline(ClaimsPrincipal user)
    {
        _httpContextAccessor.HttpContext.User = user; // note: this code auth mediatr pipeline problem solver.
        return Task.CompletedTask;
    }

    public async Task<Guid> GetUserIdFromCookie()
    {
        string token = await GetTokenValueToCookie(AccessToken);
        
        if (token == null)
            return Guid.Empty;
        
        JwtSecurityToken jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        string nameIdenfier = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier, null)?.Value ?? string.Empty;

        return Guid.Parse(nameIdenfier);
    }
}