using FeatureFolio.Application.Entries;
using FeatureFolio.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FeatureFolio.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class Test : ControllerBase
{
    private readonly IMessagePublisher _messagePublisher;

    public Test(IMessagePublisher messagePublisher)
    {
        _messagePublisher = messagePublisher;
    }

    [HttpGet]
    public async Task<IActionResult> test()
    {
        ImagesUploadedEntry entry = new ImagesUploadedEntry
        {
            TimeStamp = DateTime.UtcNow,
            ImagesUrls = ["https://image-url-1.png", "https://image-url-1.png"]
        };

        await _messagePublisher.PublishMessageAsync(entry);

        return Ok();
    }
}
