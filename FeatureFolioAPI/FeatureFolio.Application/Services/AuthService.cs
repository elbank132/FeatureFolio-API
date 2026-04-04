using FeatureFolio.Application.DTOs;
using FeatureFolio.Application.Interfaces;

namespace FeatureFolio.Application.Services;

public class AuthService : IAuthService
{
    private readonly IGoogleAuthService _googleAuthService;

    public AuthService(IGoogleAuthService googleAuthService)
    {
        _googleAuthService = googleAuthService;
    }

    public async Task<AuthResult> VerifyGoogleToken(string googleToken)
    {
        var result = await _googleAuthService.VerifyTokenAsync(googleToken);

        return result;
    }
}
