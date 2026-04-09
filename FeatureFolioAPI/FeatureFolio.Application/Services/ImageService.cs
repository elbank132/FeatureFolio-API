using FeatureFolio.Application.DTOs;
using FeatureFolio.Application.Entries;
using FeatureFolio.Application.Interfaces;
using FeatureFolio.Domain.Exceptions;

namespace FeatureFolio.Application.Services;

public class ImageService : IImageService
{
    private readonly IStorageService _storageService;
    private readonly ICacheService _cacheService;
    private readonly IMessagePublisher _messagePublisher;
    public ImageService(IStorageService storageService, ICacheService cacheService, IMessagePublisher messagePublisher)
    {
        _storageService = storageService;
        _cacheService = cacheService;
        _messagePublisher = messagePublisher;
    }

    public async Task<ImageTokensDto> GetImageSasUrlsAsync(int amount, string userGuid)
    {
        var sasList = await _storageService.GenerateSasBatchAsync(amount);

        var blobList = sasList.Select(s => s.BlobName).ToList();
        await _cacheService.SetAsync(userGuid, blobList);
        
        var tokensList = sasList.Select(s => s.UploadUrl).ToList();
        var res = new ImageTokensDto
        {
            ImageSasUrls = tokensList
        };

        return res;
    }

    public async Task ProcessFinishedUploadingAsync(string userGuid)
    {
        var imagesNamesList = await GetImagesListAsync(userGuid);
        await PublishImagesUploadedMessageAsync(userGuid, imagesNamesList);
        await _cacheService.RemoveAsync(userGuid);
    }

    private async Task<List<string>> GetImagesListAsync(string userGuid)
    {
        var imageNamesList = await _cacheService.GetAsync<List<string>>(userGuid);
        if (imageNamesList == null || imageNamesList.Count == 0)
        {
            throw new RedisInvalidDataException(userGuid);
        }

        var failedImages = await _storageService.VerifyAllBlobsExistAsync(imageNamesList);
        //notify user this file failed

        imageNamesList = imageNamesList.Where(image => !failedImages.Contains(image)).ToList();

        return imageNamesList;
    }

    private async Task PublishImagesUploadedMessageAsync(string userGuid, List<string> imagesNamesList)
    {
        var images = new ImagesUploadedEntry
        {
            EventId = "EventId",
            ImageNames = imagesNamesList,
            TimeStamp = DateTime.Now,
            UserGuid = userGuid
        };

        await _messagePublisher.PublishMessageAsync(images);
    }
}
