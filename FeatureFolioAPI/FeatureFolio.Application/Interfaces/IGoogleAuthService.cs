using FeatureFolio.Application.DTOs;

namespace FeatureFolio.Application.Interfaces;

public interface IGoogleAuthService
{
    public Task<AuthResult> VerifyTokenAsync(string credential);
}
