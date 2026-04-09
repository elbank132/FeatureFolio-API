using FeatureFolio.Application.Common.Models;

namespace FeatureFolio.Application.Interfaces;

public interface IStorageService
{
    public Task<List<ImageSas>> GenerateSasBatchAsync(int batchSize);
    public Task<List<string>> VerifyAllBlobsExistAsync(List<string> blobNames);
}
