using ImageResizer.Application.Models.Request.Thumbnail;
using ImageResizer.Application.Services.Thumbnail;
using ImageResizer.Domain.Models.Queues;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ImageResizer.ThumbnailProcessor
{
    public class ThumbnailProcessorFunction(IThumbnailService thumbnailService, ILogger<ThumbnailProcessorFunction> logger)
    {
        [Function(nameof(ThumbnailProcessorFunction))]
        public async Task RunAsync([QueueTrigger("thumbnailqueue", Connection = "AzureWebJobsStorage")] ThumbnailProcessorMessage message)
        {
            logger.LogInformation($"Processing started for the following thumbnail: {message}");

            await thumbnailService.SaveThumbnailAsync(new SaveThumbnailRequest()
            {
                FileName = message.FileName,
                FileUploadId = message.FileUploadId,
                Height = message.Height,
                Uri = message.BlobUri
            });

            logger.LogInformation($"Processing finished for the following thumbnail: {message}");
        }
    }
}
