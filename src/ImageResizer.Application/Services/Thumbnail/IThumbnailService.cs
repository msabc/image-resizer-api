using ImageResizer.Application.Models.Request.Thumbnail;

namespace ImageResizer.Application.Services.Thumbnail
{
    public interface IThumbnailService
    {
        Task SaveThumbnailAsync(SaveThumbnailRequest request);
    }
}
