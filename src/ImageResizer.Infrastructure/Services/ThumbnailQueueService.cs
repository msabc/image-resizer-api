using Azure.Storage.Queues;
using ImageResizer.Configuration;
using ImageResizer.Configuration.Models;
using ImageResizer.Domain.Interfaces.Services;
using ImageResizer.Domain.Models.Queues;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ImageResizer.Infrastructure.Services
{
    public class ThumbnailQueueService : IThumbnailQueueService
    {
        private readonly QueueClient _queueClient;

        public ThumbnailQueueService(IOptions<ResizerSettings> resizerOptions)
        {
            if (string.IsNullOrWhiteSpace(resizerOptions.Value.QueueSettings.ConnectionString))
                throw new ArgumentException($"Missing configuration setting: {nameof(QueueSettingsElement.ConnectionString)}.");

            if (string.IsNullOrWhiteSpace(resizerOptions.Value.QueueSettings.ThumbnailsQueueName))
                throw new ArgumentException($"Missing configuration setting {nameof(QueueSettingsElement.ThumbnailsQueueName)}.");

            _queueClient = new QueueClient(
                resizerOptions.Value.QueueSettings.ConnectionString,
                resizerOptions.Value.QueueSettings.ThumbnailsQueueName,
                new QueueClientOptions()
                {
                    MessageEncoding = QueueMessageEncoding.Base64
                }
            );
            
            _queueClient.CreateIfNotExists();
        }

        public async Task AddMessageAsync(ThumbnailProcessorMessage message)
        {
            await _queueClient.SendMessageAsync(JsonSerializer.Serialize(message));
        }
    }
}