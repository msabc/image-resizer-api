using Azure.Storage.Queues.Models;
using ImageResizer.Application.Models.Request.Thumbnail;
using ImageResizer.Application.Services.Thumbnail;
using ImageResizer.Domain.Models.Queues;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ImageResizer.ThumbnailProcessor
{
    public class ThumbnailProcessorFunction(
        IThumbnailService thumbnailService,
        ILogger<ThumbnailProcessorFunction> logger)
    {
        [Function(nameof(ThumbnailProcessorFunction))]
        public async Task Run([QueueTrigger("thumbnails", Connection = "AzureWebJobsStorage")] QueueMessage message)
        {
            logger.LogInformation($"{nameof(ThumbnailProcessorFunction)} trigger processing started the following message: {message.MessageText}");

            ThumbnailProcessorMessage thumbnailProcessorMessage = JsonSerializer.Deserialize<ThumbnailProcessorMessage>(message.MessageText)!;

            await thumbnailService.SaveThumbnailAsync(new SaveThumbnailRequest()
            {
                FileName = thumbnailProcessorMessage.FileName,
                FileUploadId = thumbnailProcessorMessage.FileUploadId,
                Height = thumbnailProcessorMessage.Height,
                Uri = thumbnailProcessorMessage.BlobUri
            });

            logger.LogInformation($"{nameof(ThumbnailProcessorFunction)} trigger processing finished the following message: {message.MessageText}");
        }
    }
}
