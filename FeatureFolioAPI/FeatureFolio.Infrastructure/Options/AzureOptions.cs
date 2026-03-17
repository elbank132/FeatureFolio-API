namespace FeatureFolio.Infrastructure.Options;

public class AzureOptions
{
    public const string SectionName = "Azure";
    public ServiceBusOptions serviceBusOptions { get; set; } = new ServiceBusOptions();
    public StorageOptions storageOptions { get; set; } = new StorageOptions();
}

public class ServiceBusOptions
{
    public string ServiceBusNamespace { get; set; } = String.Empty;
    public string ImagesTopicName { get; set; } = string.Empty;
}

public class StorageOptions
{
    public string StorageUrl { get; set; } = String.Empty;
}