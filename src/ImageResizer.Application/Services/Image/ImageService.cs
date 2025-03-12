using ImageResizer.Application.Mappers;
using ImageResizer.Application.Models.Response.Image;
using ImageResizer.Application.Services.Validation;
using ImageResizer.Domain.Commands;
using ImageResizer.Domain.Interfaces.Repositories;
using ImageResizer.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ImageResizer.Application.Services.Image
{
    public class ImageService(
        IFileValidationService fileValidationService,
        IBlobService blobService,
        IFileUploadRepository fileUploadRepository, 
        ILogger<ImageService> logger) : IImageService
    {
        public async Task<UploadImageResponse> UploadAsync(Guid userId, IFormFile file)
        {
            fileValidationService.ValidateImage(file);

            try
            {
                string uri = string.Empty;

                await fileUploadRepository.ExecuteInTransactionAsync(async () =>
                {
                    uri = await blobService.UploadAsync(file.OpenReadStream(), file.FileName);

                    AddFileUploadCommand command = file.MapToCommand(userId, uri);
                    command.CreatedDate = DateTime.UtcNow;

                    await fileUploadRepository.AddAsync(command);
                });

                return new UploadImageResponse()
                {
                    Id = userId,
                    Uri = uri
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"An unexpected error occurred during image upload. {ex.Message}");
                throw;
            }
        }
    }
}
