using ImageResizer.Application.Services.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ImageResizer.Application.Services.Image
{
    public class ImageService(IFileValidationService fileValidationService, ILogger<ImageService> logger) : IImageService
    {
        public async Task UploadAsync(IFormFile file)
        {
            fileValidationService.ValidateImage(file);
        }
    }
}
