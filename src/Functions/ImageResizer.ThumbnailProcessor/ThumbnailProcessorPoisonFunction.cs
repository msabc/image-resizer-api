using System.Text.Json;
using Azure.Storage.Queues.Models;
using ImageResizer.Application.Models.Request.Thumbnail;
using ImageResizer.Application.Services.Thumbnail;
using ImageResizer.Domain.Models.Queues;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ImageResizer.ThumbnailProcessor
{
    public class ThumbnailProcessorPoisonFunction(ILogger<ThumbnailProcessorPoisonFunction> logger)
    {
        [Function(nameof(ThumbnailProcessorPoisonFunction))]
        public void Run([QueueTrigger("thumbnailqueue-poison", Connection = "AzureWebJobsStorage")] ThumbnailProcessorMessage message)
        {
            logger.LogError($"Failed to create a thumbnail for the {nameof(ThumbnailProcessorMessage.FileUploadId)} {message.FileUploadId}.");
        }
    }
}
