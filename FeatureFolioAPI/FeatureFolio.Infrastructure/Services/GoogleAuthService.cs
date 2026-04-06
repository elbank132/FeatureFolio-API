using FeatureFolio.Application.DTOs;
using FeatureFolio.Application.Interfaces;
using FeatureFolio.Infrastructure.Options;
using Google.Apis.Auth;
using Microsoft.Extensions.Options;

namespace FeatureFolio.Infrastructure.Services;

public class GoogleAuthService : IGoogleAuthService
{
    private readonly GoogleOptions _options;
    private readonly string _googleClientId;

    public GoogleAuthService(IOptions<AuthOptions> options)
    {
        _options = options.Value.GoogleOptions;
        _googleClientId = _options.ClientId;
    }

    public async Task<AuthResult> VerifyTokenAsync(string credential)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = [_googleClientId]
        };

        var payload = await GoogleJsonWebSignature.ValidateAsync(credential, settings);

        return new AuthResult
        {
            UserId = payload.Subject,
            Username = payload.Name,
            Email = payload.Email,
            PictureURL = payload.Picture
        };
    }
}