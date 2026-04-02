using FeatureFolio.Application.Interfaces;
using FeatureFolio.Domain.Exceptions;

namespace FeatureFolio.Infrastructure.Services;

public class ImageService : IImageService
{
    private readonly IStorageService _storageService;
    private readonly ICacheService _cacheService;
    public ImageService(IStorageService storageService, ICacheService cacheService)
    {
        _storageService = storageService;
        _cacheService = cacheService;
    }

    public async Task<ICollection<string>> GetImageSasUrlsAsync(int amount, string userGuid)
    {
        var sasList = await _storageService.GenerateSasBatchAsync(amount);

        var blobList = sasList.Select(s => s.BlobName).ToList();
        await _cacheService.SetAsync(userGuid, blobList);
        
        var res = sasList.Select(s => s.UploadUrl).ToList();

        return res;
    }

    public async Task ProcessFinishedUploadingAsync(string userGuid)
    {
        await ValidateImagesExistAsync(userGuid);
        await _cacheService.RemoveAsync(userGuid);
    }

    private async Task ValidateImagesExistAsync(string userGuid)
    {
        var imageNamesList = await _cacheService.GetAsync<List<string>>(userGuid);
        if (imageNamesList == null || imageNamesList.Count == 0)
        {
            throw new RedisInvalidDataException(userGuid);
        }

        bool allImagesExist = await _storageService.VerifyAllBlobsExistAsync(imageNamesList);
        if (!allImagesExist) 
        {
            throw new BlobNotFoundException(imageNamesList);
        }
    }
}
