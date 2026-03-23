using Microsoft.AspNetCore.Mvc;

namespace FeatureFolio.API.Controllers;

[ApiController]
public class ImagesController : ApiBaseController
{
    [HttpGet("{amount}")]
    public async Task<IActionResult> GetImagesSas([FromRoute] int amount) {
        return Ok(amount);
    }
}
