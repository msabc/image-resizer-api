using ImageResizer.Domain.Commands;

namespace ImageResizer.Domain.Interfaces.Repositories
{
    public interface IImageRepository
    {
        Task AddAsync(AddImageCommand command);
    }
}
