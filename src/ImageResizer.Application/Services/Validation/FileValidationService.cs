using ImageResizer.Configuration;
using ImageResizer.Domain.Exceptions;
using ImageResizer.Domain.Models.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
        public void ValidateImageForUpload(IFormFile file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            var fileExtension = Path.GetExtension(file.FileName);

            if (string.IsNullOrWhiteSpace(fileExtension))
                throw new CustomHttpException("Unable to determine the extension of the uploaded file.");

            if (string.IsNullOrWhiteSpace(file.ContentType) || !file.ContentType.StartsWith("image/"))
                throw new CustomHttpException("Unsupported MIME type.", System.Net.HttpStatusCode.BadRequest);

            var imageSettings = resizerSettings.Value.ImageSettings;

            if (!imageSettings.SupportedExtensions.Contains(fileExtension.ToLower()))
                throw new CustomHttpException("Unsupported file extension.", System.Net.HttpStatusCode.BadRequest);

            long maxFileSize = imageSettings.MaxFileSizeInMB * 1024 * 1024;

            if (file.Length == 0 || file.Length > maxFileSize)
                throw new CustomHttpException($"File size exceeds the limit of {imageSettings.MaxFileSizeInMB} MB.", System.Net.HttpStatusCode.BadRequest);
        }

        public void ValidateImageForResize(FileUpload fileUpload, int resizeHeight)
        {
            if (resizeHeight >= fileUpload.Height)
                throw new CustomHttpException($"Unable to resize image. The provided height ({resizeHeight}) is greater than or equal to the original height {fileUpload.Height}.", System.Net.HttpStatusCode.BadRequest);
        }
    }
}
