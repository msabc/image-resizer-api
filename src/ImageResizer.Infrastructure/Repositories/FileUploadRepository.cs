using ImageResizer.Domain.Commands;
using ImageResizer.Domain.Interfaces.DatabaseContext;
using ImageResizer.Domain.Interfaces.Repositories;
using ImageResizer.Domain.Models.Tables;

namespace ImageResizer.Infrastructure.Repositories
{
    public class FileUploadRepository(IResizerDbContext resizerDbContext) : IFileUploadRepository
    {
        public async Task AddAsync(AddFileUploadCommand command)
        {
            FileUpload fileUpload = new()
            {
                Uri = command.Uri,
                Name = command.Name,
                CreatedDate = command.CreatedDate,
                CreatedByUserId = command.CreatedByUserId
            };

            await resizerDbContext.FileUploads.AddAsync(fileUpload);

            await resizerDbContext.SaveChangesAsync();
        }

        public async Task ExecuteInTransactionAsync(Func<Task> action)
        {
            using var transaction = await resizerDbContext.Database.BeginTransactionAsync();

            try
            {
                await action();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
