using ImageResizer.Domain.Commands.File;
using ImageResizer.Domain.Models.Tables;

namespace ImageResizer.Domain.Interfaces.Repositories
{
    public interface IFileUploadRepository
    {
        Task<Guid> AddAsync(AddFileUploadCommand command);
        
        Task<List<FileUpload>> FilterAsync(FilterFileUploadCommand command);

        Task<FileUpload?> GetByIdAsync(Guid id);

        Task UpdateAsync(Guid id, string resizedUri);

        Task DeleteAsync(Guid id);
    }
}
