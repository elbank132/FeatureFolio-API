using FeatureFolio.API.Extensions;
using FeatureFolio.Application.Interfaces;
using FeatureFolio.Domain;
using FeatureFolio.Infrastructure.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace FeatureFolio.API.Controllers;

[ApiController]
public class AuthController : ApiBaseController
{
    private readonly IAuthService _authService;
    private readonly JwtOptions _jwtOptions;

    public AuthController(IAuthService authService, IOptions<JwtOptions> jwtOptions)
    {
        _authService = authService;
        _jwtOptions = jwtOptions.Value;
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> TryGetUserData()
    {
        var authDto = User.ToAuthDto();

        return Ok(authDto);
    }

    [HttpPost("google")]
    public async Task<IActionResult> Login([FromBody] string token)
    {
        var authResult = await _authService.VerifyGoogleToken(token);
        var appJwt = _authService.GetAppJwt(authResult);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddMinutes(_jwtOptions.ExpirationInMinutes)
        };

        Response.Cookies.Append(Constants.AUTH_SESSION, appJwt, cookieOptions);

        return Ok(authResult);
    }
}
