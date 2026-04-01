using FeatureFolio.Application.Interfaces;

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

    public async Task<bool> ValidateImagesExistAsync(string userGuid)
    {
        var imageNamesList = await _cacheService.GetAsync<List<string>>(userGuid);
        if (imageNamesList == null || imageNamesList.Count == 0)
        {
            return false;
        }

        bool allImagesExist = await _storageService.VerifyAllBlobsExistAsync(imageNamesList);
        if (allImagesExist) 
        {
            await _cacheService.RemoveAsync(userGuid);
        }
        return allImagesExist;
    }
}
