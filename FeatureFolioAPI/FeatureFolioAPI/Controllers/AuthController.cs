using FeatureFolio.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FeatureFolio.API.Controllers;

[ApiController]
public class AuthController : ApiBaseController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("google")]
    public async Task<IActionResult> Login([FromBody] string token)
    {
        var authResult = await _authService.VerifyGoogleToken(token);

        // set http only cookie with token

        return Ok(authResult);
    }
}
