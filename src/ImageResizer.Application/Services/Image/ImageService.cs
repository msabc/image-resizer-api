using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ImageResizer.Application.Services.Image
{
    public class ImageService(ILogger<ImageService> logger) : IImageService
    {
        public async Task UploadAsync(IFormFile file)
        {
            return;
        }
    }
}
