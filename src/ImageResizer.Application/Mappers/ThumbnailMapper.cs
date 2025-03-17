using ImageResizer.Application.Models.Request.Thumbnail;
using ImageResizer.Domain.Commands.Thumbnail;

namespace ImageResizer.Application.Mappers
{
    internal static class ThumbnailMapper
    {
        internal static AddThumbnailCommand MapToCommand(this SaveThumbnailRequest request)
        {
            return new AddThumbnailCommand()
            {
                FileUploadId = request.FileUploadId,
                Uri = request.Uri,
                Height = request.Height
            };
        }
    }
}
