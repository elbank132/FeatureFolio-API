using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using FeatureFolio.Application.Interfaces;
using FeatureFolio.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace FeatureFolio.Infrastructure.Services;

public class ImageService : IImageService
{
    private readonly AzureOptions _azureOptions;
    private readonly BlobServiceClient _blobServiceClient;

    public ImageService(IOptions<AzureOptions> azureOptions, BlobServiceClient blobServiceClient)
    {
        _azureOptions = azureOptions.Value;
        _blobServiceClient = blobServiceClient;
    }

    public async Task<ICollection<string>> GetImageSasUrlsAsync(int amount)
    {
        string containerName = _azureOptions.storageOptions.ImagesContainerName;
        string blobName = $"{Guid.NewGuid()}.jpg";

        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(blobName);

        var userDelegationKey = await _blobServiceClient.GetUserDelegationKeyAsync(
        DateTimeOffset.UtcNow,
        DateTimeOffset.UtcNow.AddHours(1));

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = containerName,
            BlobName = blobName,
            Resource = "b", // b = blob
            StartsOn = DateTimeOffset.UtcNow,
            ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(5)
        };
        sasBuilder.SetPermissions(BlobSasPermissions.Write | BlobSasPermissions.Create);

        // 3. Construct the final URL
        var blobUriBuilder = new BlobUriBuilder(blobClient.Uri)
        {
            Sas = sasBuilder.ToSasQueryParameters(userDelegationKey, _blobServiceClient.AccountName)
        };

        return Ok(new
        {
            UploadUrl = blobUriBuilder.ToUri().ToString(),
            BlobName = blobName
        });
    }
}
