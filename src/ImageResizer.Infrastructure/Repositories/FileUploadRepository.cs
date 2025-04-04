﻿using ImageResizer.Domain.Commands.File;
using ImageResizer.Domain.Interfaces.DatabaseContext;
using ImageResizer.Domain.Interfaces.Repositories;
using ImageResizer.Domain.Models.Tables;
using Microsoft.EntityFrameworkCore;

namespace ImageResizer.Infrastructure.Repositories
{
    public class FileUploadRepository(IResizerDbContext resizerDbContext) : IFileUploadRepository
    {
        public async Task<Guid> AddAsync(AddFileUploadCommand command)
        {
            FileUpload fileUpload = new()
            {
                Uri = command.Uri,
                Name = command.Name,
                CreatedDate = command.CreatedDate,
                CreatedByUserId = command.CreatedByUserId,
                Height = command.Height,
            };

            await resizerDbContext.FileUploads.AddAsync(fileUpload);

            await resizerDbContext.SaveChangesAsync();

            return fileUpload.Id;
        }

        public async Task<List<FileUpload>> FilterAsync(FilterFileUploadCommand command)
        {
            var fileUploads = resizerDbContext.FileUploads.AsQueryable();

            if (!string.IsNullOrWhiteSpace(command.Name))
                fileUploads = fileUploads.Where(x => !string.IsNullOrWhiteSpace(x.Name) && x.Name.Contains(command.Name));

            if (command.FileExtensions != null && command.FileExtensions.Any())
                fileUploads = fileUploads.Where(x => command.FileExtensions.Contains(Path.GetExtension(x.Name)));

            if (command.CreatedAfter.HasValue)
                fileUploads = fileUploads.Where(x => x.CreatedDate > command.CreatedAfter);

            if (command.CreatedBefore.HasValue)
                fileUploads = fileUploads.Where(x => x.CreatedDate < command.CreatedBefore);

            return await fileUploads.ToListAsync();
        }

        public async Task<FileUpload?> GetByIdAsync(Guid id)
        {
            return await resizerDbContext.FileUploads.SingleAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(Guid id, string resizedUri)
        {
            var fileUpload = await resizerDbContext.FileUploads.SingleAsync(x => x.Id == id);

            fileUpload.ResizedUri = resizedUri;

            await resizerDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var fileForDeletion = await resizerDbContext.FileUploads.SingleOrDefaultAsync(x => x.Id == id);

            if (fileForDeletion != null)
            {
                resizerDbContext.FileUploads.Remove(fileForDeletion);

                await resizerDbContext.SaveChangesAsync();
            }
        }
    }
}
