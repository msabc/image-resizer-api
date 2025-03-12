using Microsoft.AspNetCore.Http;

namespace ImageResizer.Application.Models.Request.Image
{
    public record UploadFileRequest
    {
        public required IFormFile File { get; set; }
    }
}
