using System.Collections.Immutable;
using System.Security.Claims;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NArchitecture.Core.Security.JWT;

namespace Application.Services.AuthService;

public class AuthManager : IAuthService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ITokenHelper<Guid, int, Guid> _tokenHelper;
    private readonly TokenOptions _tokenOptions;
    private readonly IUserOperationClaimRepository _userOperationClaimRepository;
    private readonly IMapper _mapper;

    public AuthManager(
        IUserOperationClaimRepository userOperationClaimRepository,
        IRefreshTokenRepository refreshTokenRepository,
        ITokenHelper<Guid, int, Guid> tokenHelper,
        IConfiguration configuration,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _userOperationClaimRepository = userOperationClaimRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _tokenHelper = tokenHelper;

        const string tokenOptionsConfigurationSection = "TokenOptions";
        _tokenOptions =
            configuration.GetSection(tokenOptionsConfigurationSection).Get<TokenOptions>()
            ?? throw new NullReferenceException($"\"{tokenOptionsConfigurationSection}\" section cannot found in configuration");
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<AccessToken> CreateAccessToken(User user)
    {
        IList<OperationClaim> operationClaims = await _userOperationClaimRepository.GetOperationClaimsByUserIdAsync(user.Id);
        AccessToken accessToken = _tokenHelper.CreateToken(
            user,
            operationClaims.Select(op => (NArchitecture.Core.Security.Entities.OperationClaim<int>)op).ToImmutableList()
        );
        return accessToken;
    }

    public async Task<RefreshToken> AddRefreshToken(RefreshToken refreshToken)
    {
        RefreshToken addedRefreshToken = await _refreshTokenRepository.AddAsync(refreshToken);
        return addedRefreshToken;
    }

    public async Task DeleteOldRefreshTokens(Guid userId)
    {
        List<RefreshToken> refreshTokens = await _refreshTokenRepository.GetOldRefreshTokensAsync(
            userId,
            _tokenOptions.RefreshTokenTTL
        );
        await _refreshTokenRepository.DeleteRangeAsync(refreshTokens);
    }

    public async Task<RefreshToken?> GetRefreshTokenByToken(string token)
    {
        RefreshToken? refreshToken = await _refreshTokenRepository.GetAsync(predicate: r => r.Token == token);
        return refreshToken;
    }

    public async Task RevokeRefreshToken(
        RefreshToken refreshToken,
        string ipAddress,
        string? reason = null,
        string? replacedByToken = null
    )
    {
        refreshToken.RevokedDate = DateTime.UtcNow;
        refreshToken.RevokedByIp = ipAddress;
        refreshToken.ReasonRevoked = reason;
        refreshToken.ReplacedByToken = replacedByToken;
        await _refreshTokenRepository.UpdateAsync(refreshToken);
    }

    public async Task<RefreshToken> RotateRefreshToken(User user, RefreshToken refreshToken, string ipAddress)
    {
        NArchitecture.Core.Security.Entities.RefreshToken<Guid, Guid> newCoreRefreshToken = _tokenHelper.CreateRefreshToken(
            user,
            ipAddress
        );
        RefreshToken newRefreshToken = _mapper.Map<RefreshToken>(newCoreRefreshToken);
        await RevokeRefreshToken(refreshToken, ipAddress, reason: "Replaced by new token", newRefreshToken.Token);
        return newRefreshToken;
    }

    public async Task RevokeDescendantRefreshTokens(RefreshToken refreshToken, string ipAddress, string reason)
    {
        RefreshToken? childToken = await _refreshTokenRepository.GetAsync(predicate: r =>
            r.Token == refreshToken.ReplacedByToken
        );

        if (childToken?.RevokedDate != null && childToken.ExpirationDate <= DateTime.UtcNow)
            await RevokeRefreshToken(childToken, ipAddress, reason);
        else
            await RevokeDescendantRefreshTokens(refreshToken: childToken!, ipAddress, reason);
    }

    public Task<RefreshToken> CreateRefreshToken(User user, string ipAddress)
    {
        NArchitecture.Core.Security.Entities.RefreshToken<Guid, Guid> coreRefreshToken = _tokenHelper.CreateRefreshToken(
            user,
            ipAddress
        );
        RefreshToken refreshToken = _mapper.Map<RefreshToken>(coreRefreshToken);
        return Task.FromResult(refreshToken);
    }

    public Task SetAccessTokenToCookies(AccessToken accessToken)
    {
        CookieOptions cookieOptions = new() { HttpOnly = true, Secure = true, Expires = accessToken.ExpirationDate };

        _httpContextAccessor.HttpContext.Response.Cookies.Append(nameof(accessToken), accessToken.Token);
        return Task.CompletedTask;
    }

    public Task SetRefreshTokenToCookies(RefreshToken? refreshToken)
    {
        if (refreshToken == null)
            return Task.CompletedTask;

        CookieOptions cookieOptions = new() { HttpOnly = true, Secure = true, Expires = refreshToken.ExpirationDate, };
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

    public Task Logout()
    {
        _httpContextAccessor.HttpContext.Response.Cookies.Delete("refreshToken");
        _httpContextAccessor.HttpContext.Response.Cookies.Delete("accessToken");

        return Task.CompletedTask;
    }

    public Task AddUserToAuthPipeline(ClaimsPrincipal user)
    {
        _httpContextAccessor.HttpContext.User = user; // note: this code auth mediatr pipeline problem solver.
        return Task.CompletedTask;
    }
}
