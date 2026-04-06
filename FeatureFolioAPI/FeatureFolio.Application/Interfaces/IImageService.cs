using FeatureFolio.Application.DTOs;

namespace FeatureFolio.Application.Interfaces;

public interface IImageService
{
    public Task<ImageTokensDto> GetImageSasUrlsAsync(int amount, string userGuid);
    public Task ProcessFinishedUploadingAsync(string userGuid);
}
