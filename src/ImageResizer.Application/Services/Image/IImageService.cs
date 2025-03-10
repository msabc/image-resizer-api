using Microsoft.AspNetCore.Http;

namespace ImageResizer.Application.Services.Image
{
    public interface IImageService
    {
        public Task UploadAsync(IFormFile file);
    }
}
