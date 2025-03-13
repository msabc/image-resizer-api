using ImageResizer.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Solarnelle.Domain.Exceptions;

namespace ImageResizer.Application.Services.Validation
{
    public class FileValidationService(IOptions<ResizerSettings> resizerSettings, ILogger<FileValidationService> logger) : IFileValidationService
    {
        /// <summary>
        /// Validates that the <paramref name="file"/> is an image.
        /// </summary>
        /// <param name="file">The file that should be uploaded.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="CustomHttpException"></exception>
        public async Task ValidateImageAsync(IFormFile file)
        {
            ArgumentNullException.ThrowIfNull(file);

            var fileExtension = Path.GetExtension(file.FileName);

            ArgumentNullException.ThrowIfNullOrWhiteSpace(fileExtension);

            if (string.IsNullOrWhiteSpace(file.ContentType) || !file.ContentType.StartsWith("image/"))
                throw new CustomHttpException("Unsupported MIME type.", System.Net.HttpStatusCode.BadRequest);

            var imageSettings = resizerSettings.Value.ImageSettings;

            if (!imageSettings.SupportedExtensions.Contains(fileExtension.ToLower()))
                throw new CustomHttpException("Unsupported file extension.", System.Net.HttpStatusCode.BadRequest);

            long maxFileSize = imageSettings.MaxFileSizeInMB * 1024 * 1024;

            if (file.Length == 0 || file.Length > maxFileSize)
                throw new CustomHttpException($"File size exceeds the limit of {imageSettings.MaxFileSizeInMB} MB.", System.Net.HttpStatusCode.BadRequest);

            try
            {
                await SixLabors.ImageSharp.Image.LoadAsync(file.OpenReadStream());
            }
            catch (Exception ex)
            {
                throw new CustomHttpException("Unsupported content.", ex, System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}
