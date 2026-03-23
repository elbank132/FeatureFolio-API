using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using FeatureFolio.Application.Interfaces;
using FeatureFolio.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace FeatureFolio.Infrastructure.Services;

public class ImageService : IImageService
{
    private readonly IStorageService _storageService;

    public ImageService(IStorageService storageService)
    {
        _storageService = storageService;
    }

    public async Task<ICollection<string>> GetImageSasUrlsAsync(int amount)
    {
        var sasList = await _storageService.GenerateSasBatchAsync(amount);

        var res = sasList.Select(s => s.UploadUrl).ToList();

        return res;
    }
}
