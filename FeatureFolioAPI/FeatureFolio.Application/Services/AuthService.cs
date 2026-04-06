using FeatureFolio.Application.DTOs;
using FeatureFolio.Application.Interfaces;

namespace FeatureFolio.Application.Services;

public class AuthService : IAuthService
{
    private readonly IGoogleAuthService _googleAuthService;
    private readonly ITokenService _tokenService;

    public AuthService(IGoogleAuthService googleAuthService, ITokenService tokenService)
    {
        _googleAuthService = googleAuthService;
        _tokenService = tokenService;
    }

    public string GetAppJwt(AuthResult authResult)
    {
        var token = _tokenService.GenerateToken(authResult);

        return token;
    }

    public async Task<AuthResult> VerifyGoogleToken(string googleToken)
    {
        var result = await _googleAuthService.VerifyTokenAsync(googleToken);

        return result;
    }
}
