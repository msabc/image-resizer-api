using ImageResizer.Domain.Commands;

namespace ImageResizer.Domain.Interfaces.Repositories
{
    public interface IFileUploadRepository
    {
        Task AddAsync(AddFileUploadCommand command);
    }
}
