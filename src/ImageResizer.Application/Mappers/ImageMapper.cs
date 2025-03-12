using ImageResizer.Domain.Commands;
using Microsoft.AspNetCore.Http;

namespace ImageResizer.Application.Mappers
{
    internal static class ImageMapper
    {
        internal static AddFileUploadCommand MapToCommand(this IFormFile file, Guid userId, string uri)
        {
            return new AddFileUploadCommand()
            {
                Name = file.FileName,
                CreatedByUserId = userId,
                Uri = uri
            };
        }
    }
}
