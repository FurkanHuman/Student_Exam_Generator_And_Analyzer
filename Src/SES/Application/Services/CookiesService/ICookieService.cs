using Domain.Entities;
using NArchitecture.Core.Security.JWT;
using System.Security.Claims;

namespace Application.Services.CookiesService;
public interface ICookieService
{
    public Task SetAccessTokenToCookies(AccessToken accessToken);
    public Task SetRefreshTokenToCookies(RefreshToken? refreshToken);
    public Task<string> GetTokenValueToCookie(string val);
    public Task<string> GetIpV4AndIpV6Client();
    public Task CookieLogout();
    public Task AddUserToAuthPipeline(ClaimsPrincipal user);
    public Task<Guid> GetUserIdFromCookie();
}
