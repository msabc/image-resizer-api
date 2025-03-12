using ImageResizer.Application.Models.Response.Image;
using Microsoft.AspNetCore.Http;

namespace ImageResizer.Application.Services.Image
{
    public interface IImageService
    {
        public Task<UploadImageResponse> UploadAsync(Guid userId, IFormFile file);
    }
}
