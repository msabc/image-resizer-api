using System.Text.Encodings.Web;
using ImageResizer.Application.Mappers;
using ImageResizer.Application.Models.Request.Image;
using ImageResizer.Application.Models.Response.Image;
using ImageResizer.Application.Services.Validation;
using ImageResizer.Configuration;
using ImageResizer.Domain.Commands.File;
using ImageResizer.Domain.Exceptions;
using ImageResizer.Domain.Interfaces.Repositories;
using ImageResizer.Domain.Interfaces.Services;
using ImageResizer.Domain.Interfaces.Transactions;
using ImageResizer.Domain.Models.Queues;
using ImageResizer.Domain.Models.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ImageResizer.Application.Services.Image
{
    public class ImageService(
        IFileValidationService fileValidationService,
        IImageBlobService imageBlobService,
        IFileUploadRepository fileUploadRepository,
        IImageProcessorService imageProcessorService,
        IResizerTransactionExecutor transactionExecutor,
        IThumbnailQueueService thumbnailQueueService,
        IOptions<ResizerSettings> resizerOptions) : IImageService
    {
        public async Task<UploadImageResponse> UploadAsync(Guid userId, IFormFile file)
        {
            fileValidationService.ValidateImageForUpload(file);

            int imageHeight = 0;
            string uri = string.Empty;
            Guid insertedId = Guid.Empty;

            try
            {
                (int height, int width) = await imageProcessorService.LoadAsync(file.OpenReadStream());

                imageHeight = height;
            }
            catch (Exception ex)
            {
                throw new CustomHttpException("Unsupported content.", ex, System.Net.HttpStatusCode.BadRequest);
            }

            if (imageHeight == 0)
                throw new CustomHttpException("Invalid image.", System.Net.HttpStatusCode.BadRequest);

            await transactionExecutor.ExecuteInTransactionAsync(async () =>
            {
                uri = await imageBlobService.UploadAsync(file.OpenReadStream(), GetBlobUploadName(userId));

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

            await thumbnailQueueService.AddMessageAsync(new ThumbnailProcessorMessage()
            {
                FileUploadId = insertedId,
                FileName = file.FileName,
                BlobUri = uri,
                Height = resizerOptions.Value.ImageSettings.DefaultThumbnailHeightInPixels
            });

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

            Stream fileStream = await imageBlobService.DownloadAsync(new Uri(fileUpload.Uri));

            string extension = Path.GetExtension(fileUpload.Name);

            using MemoryStream memoryStream = new();
            await imageProcessorService.ResizeAsync(fileStream, memoryStream, extension, request.Height);
            memoryStream.Position = 0;

            string uri = string.Empty;

            await transactionExecutor.ExecuteInTransactionAsync(async () =>
            {
                uri = await imageBlobService.UploadAsync(memoryStream, GetBlobUploadName(userId));
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
                throw new CustomHttpException($"Unable to find an image with id {id}.", System.Net.HttpStatusCode.NotFound);

            await transactionExecutor.ExecuteInTransactionAsync(async () =>
            {
                await imageBlobService.DeleteAsync(file.Uri);

                if (!string.IsNullOrWhiteSpace(file.ResizedUri))
                    await imageBlobService.DeleteAsync(file.ResizedUri);

                await fileUploadRepository.DeleteAsync(file.Id);
            });
        }

        public async Task<GenerateSasUrlResponse> GenerateSasUrlAsync(Guid userId, Guid id)
        {
            var fileUpload = await fileUploadRepository.GetByIdAsync(id);

            if (fileUpload == null)
                throw new CustomHttpException($"No image with id {id} exists.", System.Net.HttpStatusCode.NotFound);

            if (fileUpload.CreatedByUserId != userId)
                throw new CustomHttpException($"You do not have the permission to view this image.", System.Net.HttpStatusCode.Forbidden);
            
            return new GenerateSasUrlResponse()
            {
                Url = $"{fileUpload.Uri}?{imageBlobService.GenerateSasToken(fileUpload.Uri, fileUpload.Name)}"
            };
        }

        private static string GetBlobUploadName(Guid userId) => $"{userId}/{Guid.NewGuid()}";
    }
}
