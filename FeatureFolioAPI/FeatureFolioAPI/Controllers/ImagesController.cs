using FeatureFolio.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FeatureFolio.API.Controllers;

[ApiController]
public class ImagesController : ApiBaseController
{
    private readonly IImageService _imageService;
    private readonly ICacheService cacheService;

    public ImagesController(IImageService imageService, ICacheService cacheService)
    {
        _imageService = imageService;
        this.cacheService = cacheService;
    }

    [HttpGet("{amount}")]
    public async Task<IActionResult> GetImagesSas([FromRoute] int amount) {
        var sasUrls = await _imageService.GetImageSasUrlsAsync(amount);

        return Ok(sasUrls);
    }
}
