using ImageResizer.Domain.Commands.File;
using ImageResizer.Domain.Models.Tables;

namespace ImageResizer.Domain.Interfaces.Repositories
{
    public interface IFileUploadRepository
    {
        Task AddAsync(AddFileUploadCommand command);
        
        Task<List<FileUpload>> FilterAsync(FilterFileUploadCommand command);

        Task<FileUpload?> GetByIdAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task ExecuteInTransactionAsync(Func<Task> action);
    }
}
