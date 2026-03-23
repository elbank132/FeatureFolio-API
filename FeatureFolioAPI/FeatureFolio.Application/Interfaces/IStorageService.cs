using FeatureFolio.Application.Common.Models;

namespace FeatureFolio.Application.Interfaces;

public interface IStorageService
{
    public Task<List<ImageSas>> GenerateSasBatch(int batchSize);
}
