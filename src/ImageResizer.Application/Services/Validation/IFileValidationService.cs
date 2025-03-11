using Microsoft.AspNetCore.Http;

namespace ImageResizer.Application.Services.Validation
{
    public interface IFileValidationService
    {
        void ValidateImage(IFormFile file);
    }
}
