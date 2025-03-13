using Microsoft.AspNetCore.Http;

namespace ImageResizer.Application.Services.Validation
{
    public interface IFileValidationService
    {
        Task ValidateImageAsync(IFormFile file);
    }
}
