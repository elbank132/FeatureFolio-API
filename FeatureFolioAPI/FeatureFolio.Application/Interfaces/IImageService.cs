namespace FeatureFolio.Application.Interfaces;

public interface IImageService
{
    public Task<ICollection<string>> GetImageSasUrlsAsync(int amount , string userGuid);
    public Task<bool> ValidateImagesExistAsync(string userGuid);
}
