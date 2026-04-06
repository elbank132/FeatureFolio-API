using FeatureFolio.Application.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;

namespace FeatureFolio.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static AuthDto ToAuthDto(this ClaimsPrincipal user)
    {
        return new AuthDto
        {
            PictureURL = user.FindFirstValue(JwtRegisteredClaimNames.Picture) ?? string.Empty,
            Username = user.FindFirstValue(JwtRegisteredClaimNames.Name) ?? string.Empty,
        };
    }
}
