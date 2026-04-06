using FeatureFolio.Application.DTOs;

namespace FeatureFolio.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(AuthResult authResult);
}
