using ImageResizer.Domain.Models.Queues;

namespace ImageResizer.Domain.Interfaces.Services
{
    public interface IThumbnailQueueService
    {
        Task AddMessageAsync(ThumbnailProcessorMessage message);
    }
}
