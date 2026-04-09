using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using FeatureFolio.Application.Common.Models;
using FeatureFolio.Application.Interfaces;
using FeatureFolio.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace FeatureFolio.Infrastructure.Services;

public class StorageService : IStorageService
{
    private readonly AzureOptions _azureOptions;
    private readonly BlobServiceClient _blobServiceClient;

    public StorageService(IOptions<AzureOptions> azureOptions, BlobServiceClient blobServiceClient)
    {
        _azureOptions = azureOptions.Value;
        _blobServiceClient = blobServiceClient;
    }

    public async Task<List<ImageSas>> GenerateSasBatchAsync(int batchSize)
    {
        string containerName = _azureOptions.storageOptions.ImagesContainerName;
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

        var userDelegationKey = await _blobServiceClient.GetUserDelegationKeyAsync(
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow.AddHours(1)
            );

        List<ImageSas> imageSasList = []; 

        for (int i = 0; i < batchSize; i++)
        {
            var imageSas = CreateSasToken(containerClient, containerName, userDelegationKey);

            imageSasList.Add(imageSas);
        }

        return imageSasList;
    }

    private ImageSas CreateSasToken(
        BlobContainerClient containerClient, 
        string containerName, 
        Azure.Response<UserDelegationKey> userDelegationKey
        )
    {
        string blobName = $"{Guid.NewGuid()}.jpg";
        var blobClient = containerClient.GetBlobClient(blobName);

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = containerName,
            BlobName = blobName,
            Resource = "b", // b = blob
            StartsOn = DateTimeOffset.UtcNow,
            ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(15)
        };
        sasBuilder.SetPermissions(BlobSasPermissions.Write | BlobSasPermissions.Create);

        var blobUriBuilder = new BlobUriBuilder(blobClient.Uri)
        {
            Sas = sasBuilder.ToSasQueryParameters(userDelegationKey, _blobServiceClient.AccountName)
        };

        ImageSas imageSas = new ImageSas
        {
            UploadUrl = blobUriBuilder.ToUri().ToString(),
            BlobName = blobName
        };

        return imageSas;
    }

    public async Task<List<string>> VerifyAllBlobsExistAsync(List<string> blobNames)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_azureOptions.storageOptions.ImagesContainerName);

        var tasks = blobNames.Select(async name =>
        {
            var blobClient = containerClient.GetBlobClient(name);
            var response = await blobClient.ExistsAsync();

            return (Name: name, Exists: response.Value);
        });

        var results = await Task.WhenAll(tasks);

        var missingBlobs = results
            .Where(r => r.Exists == false)
            .Select(r => r.Name)
            .ToList();

        return missingBlobs;
    }

}
