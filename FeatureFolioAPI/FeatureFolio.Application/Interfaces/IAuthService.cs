using FeatureFolio.Application.DTOs;

namespace FeatureFolio.Application.Interfaces;

public interface IAuthService
{
    public Task<AuthResult> VerifyGoogleToken(string googleToken);
    public string GetAppJwt(AuthResult authResult);
}
