﻿using ImageResizer.Application.Models.Request.Image;
using ImageResizer.Application.Models.Response.Image;
using Microsoft.AspNetCore.Http;

namespace ImageResizer.Application.Services.Image
{
    public interface IImageService
    {
        public Task<UploadImageResponse> UploadAsync(Guid userId, IFormFile file);

        public Task<FilterImagesReponse> FilterAsync(Guid userId, FilterImagesRequest request);

        public Task<GetByIdResponse> GetByIdAsync(Guid userId, Guid id);

        public Task DeleteAsync(Guid userId, Guid id);
    }
}
