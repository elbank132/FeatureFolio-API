using FeatureFolio.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FeatureFolio.API.Controllers;

[ApiController]
public class ImagesController : ApiBaseController
{
    private readonly IImageService _imageService;

    public ImagesController(IImageService imageService)
    {
        _imageService = imageService;
    }

    [Authorize]
    [HttpGet("{amount}")]
    public async Task<IActionResult> GetImagesSas([FromRoute] int amount) {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException(); 
        var sasUrls = await _imageService.GetImageSasUrlsAsync(amount, userId);

        return Ok(sasUrls);
    }

    [Authorize]
    [HttpPost("finished")]
    public async Task<IActionResult> FinishedUploading()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException();
        await _imageService.ProcessFinishedUploadingAsync(userId);

        return NoContent();
    }
}
