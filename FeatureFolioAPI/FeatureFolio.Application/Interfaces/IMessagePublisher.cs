using FeatureFolio.Application.Entries;

namespace FeatureFolio.Application.Interfaces;

public interface IMessagePublisher
{
    public Task PublishMessageAsync(ImagesUploadedEntry images);
}
