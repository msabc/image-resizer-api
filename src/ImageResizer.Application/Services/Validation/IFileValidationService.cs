using ImageResizer.Domain.Models.Tables;
using Microsoft.AspNetCore.Http;

namespace ImageResizer.Application.Services.Validation
{
    public interface IFileValidationService
    {
        void ValidateImageForUpload(IFormFile file);

        void ValidateImageForResize(FileUpload fileUpload, int resizeHeight);
    }
}
