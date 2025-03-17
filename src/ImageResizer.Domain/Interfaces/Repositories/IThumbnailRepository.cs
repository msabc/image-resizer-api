using ImageResizer.Domain.Commands.Thumbnail;

namespace ImageResizer.Domain.Interfaces.Repositories
{
    public interface IThumbnailRepository
    {
        Task AddAsync(AddThumbnailCommand command);
    }
}
