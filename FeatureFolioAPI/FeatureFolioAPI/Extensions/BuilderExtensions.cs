using Azure.Identity;
using Microsoft.Extensions.Azure;

namespace FeatureFolio.API.Extensions;

public static class BuilderExtensions
{
    public static IServiceCollection AddAzureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddAzureClients(clientBuilder =>
        {
            // Apply DefaultAzureCredential to all registered clients
            clientBuilder.UseCredential(new DefaultAzureCredential());

            // Register Blob Storage
            var storageUrl = config["Azure:StorageUrl"];
            if (!string.IsNullOrEmpty(storageUrl))
            {
                clientBuilder.AddBlobServiceClient(new Uri(storageUrl));
            }

            // Register Service Bus
            var serviceBusNamespace = config["Azure:ServiceBusNamespace"];
            if (!string.IsNullOrEmpty(serviceBusNamespace))
            {
                clientBuilder.AddServiceBusClientWithNamespace(serviceBusNamespace);
            }

            services.AddHealthChecks()
                // 1. Azure Blob Storage Health Check
                .AddAzureBlobStorage(
                    new Uri(config["Azure:StorageUrl"]!),
                    new DefaultAzureCredential(),
                    name: "BlobStorageCheck")

                // 2. Azure Service Bus Queue Health Check
                .AddAzureServiceBusTopic(
                    config["Azure:ServiceBusNamespace"]!,
                    "process-images-topic", // Replace with your actual queue name
                    new DefaultAzureCredential(),
                    name: "ServiceBusCheck");
        });

        return services;
    }
}
