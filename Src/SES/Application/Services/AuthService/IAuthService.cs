using Application.Features.Auth.Commands.Login;
using Domain.Entities;
using NArchitecture.Core.Security.JWT;

namespace Application.Services.AuthService;

public interface IAuthService
{
    public Task<AccessToken> CreateAccessToken(User user);
    public Task<RefreshToken> CreateRefreshToken(User user, string ipAddress);
    public Task<RefreshToken?> GetRefreshTokenByToken(string refreshToken);
    public Task<RefreshToken> AddRefreshToken(RefreshToken refreshToken);
    public Task DeleteOldRefreshTokens(Guid userId);
    public Task RevokeDescendantRefreshTokens(RefreshToken refreshToken, string ipAddress, string reason);
    public Task RevokeRefreshToken(RefreshToken token, string ipAddress, string? reason = null, string? replacedByToken = null);
    public Task<RefreshToken> RotateRefreshToken(User user, RefreshToken refreshToken, string ipAddress);
    public Task SetAccessTokenToCookies(AccessToken accessToken);
    public Task SetRefreshTokenToCookies(RefreshToken? refreshToken);
    public Task<string> GetTokenValueToCookie(string val);
    public Task<string> GetIpV4AndIpV6Client();
    public Task Logout();
}
