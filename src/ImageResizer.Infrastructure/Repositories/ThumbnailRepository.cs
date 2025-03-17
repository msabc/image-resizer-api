using ImageResizer.Domain.Commands.Thumbnail;
using ImageResizer.Domain.Interfaces.DatabaseContext;
using ImageResizer.Domain.Interfaces.Repositories;
using ImageResizer.Domain.Models.Tables;

namespace ImageResizer.Infrastructure.Repositories
{
    public class ThumbnailRepository(IResizerDbContext resizerDbContext) : IThumbnailRepository
    {
        public async Task AddAsync(AddThumbnailCommand command)
        {
            Thumbnail thumbnail = new()
            {
                FileUploadId = command.FileUploadId,
                Uri = command.Uri,
                Height = command.Height
            };

            await resizerDbContext.Thumbnails.AddAsync(thumbnail);

            await resizerDbContext.SaveChangesAsync();
        }
    }
}
