using FeatureFolio.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FeatureFolio.API.Controllers;

[ApiController]
public class ImagesController : ApiBaseController
{
    private readonly IImageService _imageService;

    public ImagesController(IImageService imageService)
    {
        _imageService = imageService;
    }

    [HttpGet("{amount}")]
    public async Task<IActionResult> GetImagesSas([FromRoute] int amount) {
        var sasUrls = await _imageService.GetImageSasUrlsAsync(amount);

        return Ok(sasUrls);
    }
}
