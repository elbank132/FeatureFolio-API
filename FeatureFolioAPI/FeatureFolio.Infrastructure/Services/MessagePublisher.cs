using Azure.Messaging.ServiceBus;
using FeatureFolio.Application.Entries;
using FeatureFolio.Application.Interfaces;
using FeatureFolio.Infrastructure.Options;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace FeatureFolio.Infrastructure.Services;

public class MessagePublisher : IMessagePublisher
{
    private readonly ServiceBusSender _sender;
    private readonly AzureOptions _options;

    public MessagePublisher(IOptions<AzureOptions> options, ServiceBusClient client)
    {
        _options = options.Value;
        var topicName = _options.serviceBusOptions.ImagesTopicName;
        _sender = client.CreateSender(topicName);
    }

    public async Task PublishMessageAsync(ImagesUploadedEntry images)
    {
        await SerializeAndSendAsync(images);
    }

    private async Task SerializeAndSendAsync(object payload)
    {
        string jsonPayload = JsonSerializer.Serialize(payload);
        ServiceBusMessage message = new ServiceBusMessage(jsonPayload);
        await _sender.SendMessageAsync(message);
    }
}
