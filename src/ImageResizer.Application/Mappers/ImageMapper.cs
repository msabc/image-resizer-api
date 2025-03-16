using ImageResizer.Application.Models.Request.Image;
using ImageResizer.Application.Models.Response.Image;
using ImageResizer.Domain.Commands.File;
using ImageResizer.Domain.Models.Tables;
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
                Uri = uri,
                CreatedDate = DateTime.UtcNow,
            };
        }

        internal static FilterFileUploadCommand MapToCommand(this FilterImagesRequest request, Guid userId)
        {
            return new FilterFileUploadCommand()
            {
                CreatedByUserId = userId,
                Name = request.Name,
                FileExtensions = request.FileExtensions,
                CreatedAfter = request.CreatedAfter,
                CreatedBefore = request.CreatedBefore
            };
        }

        internal static FilterImageDto MapToDto(this FileUpload fileUpload)
        {
            return new FilterImageDto()
            {
                Id = fileUpload.Id,
                CreatedDate = fileUpload.CreatedDate,
                FileName = fileUpload.Name,
                Uri = fileUpload.Uri
            };
        }

        internal static GetByIdResponse MapToResponse(this FileUpload fileUpload)
        {
            return new GetByIdResponse()
            {
                Id = fileUpload.Id,
                CreatedDate = fileUpload.CreatedDate,
                FileName = fileUpload.Name,
                Uri = fileUpload.Uri
            };
        }
    }
}
