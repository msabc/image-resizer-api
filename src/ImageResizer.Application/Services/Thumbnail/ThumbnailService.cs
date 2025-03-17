using ImageResizer.Application.Mappers;
using ImageResizer.Application.Models.Request.Thumbnail;
using ImageResizer.Domain.Interfaces.Repositories;
using ImageResizer.Domain.Interfaces.Services;
using ImageResizer.Domain.Interfaces.Transactions;

namespace ImageResizer.Application.Services.Thumbnail
{
    public class ThumbnailService(
        IThumbnailBlobService thumbnailBlobService,
        IImageProcessorService imageProcessorService,
        IResizerTransactionExecutor transactionExecutor,
        IThumbnailRepository thumbnailRepository) : IThumbnailService
    {
        public async Task SaveThumbnailAsync(SaveThumbnailRequest request)
        {
            Stream fileStream = await thumbnailBlobService.DownloadAsync(new Uri(request.Uri));
            string extension = Path.GetExtension(request.FileName);

            MemoryStream memoryStream = new();
            await imageProcessorService.ResizeAsync(fileStream, memoryStream, extension, request.Height);
            memoryStream.Position = 0;

            string uri = string.Empty;

            await transactionExecutor.ExecuteInTransactionAsync(async () =>
            {
                uri = await thumbnailBlobService.UploadAsync(memoryStream, Guid.NewGuid().ToString());
                await thumbnailRepository.AddAsync(request.MapToCommand());
            });
        }
    }
}
