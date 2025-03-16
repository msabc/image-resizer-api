using ImageResizer.Application.Mappers;
using ImageResizer.Application.Models.Request.Image;
using ImageResizer.Application.Models.Response.Image;
using ImageResizer.Application.Services.ImageEncoding;
using ImageResizer.Application.Services.Validation;
using ImageResizer.Domain.Commands.File;
using ImageResizer.Domain.Exceptions;
using ImageResizer.Domain.Interfaces.Repositories;
using ImageResizer.Domain.Interfaces.Services;
using ImageResizer.Domain.Models.Tables;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp.Processing;

namespace ImageResizer.Application.Services.Image
{
    public class ImageService(
        IFileValidationService fileValidationService,
        IBlobService blobService,
        IFileUploadRepository fileUploadRepository,
        IImageEncodingService imageEncodingService) : IImageService
    {
        public async Task<UploadImageResponse> UploadAsync(Guid userId, IFormFile file)
        {
            fileValidationService.ValidateImageForUpload(file);

            int imageHeight = 0;
            string uri = string.Empty;
            Guid insertedId = Guid.Empty;

            try
            {
                var image = await SixLabors.ImageSharp.Image.LoadAsync(file.OpenReadStream());

                imageHeight = image.Height;
            }
            catch (Exception ex)
            {
                throw new CustomHttpException("Unsupported content.", ex, System.Net.HttpStatusCode.BadRequest);
            }

            await fileUploadRepository.ExecuteInTransactionAsync(async () =>
            {
                uri = await blobService.UploadAsync(file.OpenReadStream(), Guid.NewGuid().ToString());

                AddFileUploadCommand command = new()
                {
                    Name = file.FileName,
                    CreatedByUserId = userId,
                    Uri = uri,
                    CreatedDate = DateTime.UtcNow,
                    Height = imageHeight
                };

                insertedId = await fileUploadRepository.AddAsync(command);
            });

            if (string.IsNullOrWhiteSpace(uri) || insertedId == Guid.Empty)
                throw new CustomHttpException("An unexpected error occurred.");

            return new UploadImageResponse()
            {
                Id = insertedId,
                Uri = uri
            };
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
                throw new CustomHttpException($"Unable to find an image with id {id}.", System.Net.HttpStatusCode.NotFound);

            return fileUpload.MapToResponse();
        }

        public async Task<ResizeResponse> ResizeAsync(Guid userId, ResizeRequest request)
        {
            FileUpload? fileUpload = await fileUploadRepository.GetByIdAsync(request.Id);

            if (fileUpload == null)
                throw new CustomHttpException($"Unable to find an image with id {request.Id}.", System.Net.HttpStatusCode.NotFound);

            fileValidationService.ValidateImageForResize(fileUpload, request.Height);

            Stream fileStream = await blobService.DownloadAsync(new Uri(fileUpload.Uri));

            using var image = await SixLabors.ImageSharp.Image.LoadAsync(fileStream);

            float aspectRatio = (float)image.Width / image.Height;
            int newWidth = (int)(request.Height * aspectRatio);

            image.Mutate(x => x.Resize(newWidth, request.Height));

            string extension = Path.GetExtension(fileUpload.Name);

            using var outputStream = new MemoryStream();
            await image.SaveAsync(outputStream, imageEncodingService.GetImageEncoder(extension));
            outputStream.Position = 0;

            string uri = string.Empty;

            await fileUploadRepository.ExecuteInTransactionAsync(async () =>
            {
                uri = await blobService.UploadAsync(outputStream, Guid.NewGuid().ToString());
                await fileUploadRepository.UpdateAsync(fileUpload.Id, uri);
            });

            return new ResizeResponse()
            {
                Uri = uri
            };
        }

        public async Task DeleteAsync(Guid userId, Guid id)
        {
            var file = await fileUploadRepository.GetByIdAsync(id);

            if (file == null)
                throw new CustomHttpException($"Unable to find an image with id {id}", System.Net.HttpStatusCode.NotFound);

            await fileUploadRepository.ExecuteInTransactionAsync(async () =>
            {
                await blobService.DeleteAsync(file.Uri);

                if (!string.IsNullOrWhiteSpace(file.ResizedUri))
                    await blobService.DeleteAsync(file.ResizedUri);

                await fileUploadRepository.DeleteAsync(file.Id);
            });
        }
    }
}
