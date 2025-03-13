using ImageResizer.Application.Mappers;
using ImageResizer.Application.Models.Request.Image;
using ImageResizer.Application.Models.Response.Image;
using ImageResizer.Application.Services.Validation;
using ImageResizer.Domain.Commands.File;
using ImageResizer.Domain.Exceptions;
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
            try
            {
                await fileValidationService.ValidateImageAsync(file);

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

        public async Task<FilterImagesReponse> FilterAsync(Guid userId, FilterImagesRequest request)
        {
            var command = request.MapToCommand(userId);

            var response = await fileUploadRepository.FilterAsync(command);

            return new FilterImagesReponse()
            {
                Images = response.Select(x => x.MapToDto()).ToList()
            };
        }

        public async Task<GetByIdResponse> GetByIdAsync(Guid userId, Guid id)
        {
            var fileUpload = await fileUploadRepository.GetByIdAsync(id);

            if (fileUpload == null)
                throw new CustomHttpException($"Unable to find an image with id {id}", System.Net.HttpStatusCode.NotFound);

            return fileUpload.MapToResponse();
        }

        public async Task DeleteAsync(Guid userId, Guid id)
        {
            var file = await fileUploadRepository.GetByIdAsync(id);

            if (file == null)
                throw new CustomHttpException($"Unable to find an image with id {id}", System.Net.HttpStatusCode.NotFound);

            try
            {
                await fileUploadRepository.ExecuteInTransactionAsync(async () =>
                {
                    await fileUploadRepository.DeleteAsync(file.Id);

                    await blobService.DeleteAsync(file.Name);
                });
            }
            catch (Exception ex)
            {
                logger.LogError($"An unexpected error occurred during image upload. {ex.Message}");
                throw;
            }
        }
    }
}
