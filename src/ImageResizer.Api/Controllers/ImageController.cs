using ImageResizer.Api.Controllers.Base;
using ImageResizer.Application.Models.Request.Image;
using ImageResizer.Application.Models.Response.Image;
using ImageResizer.Application.Services.Image;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImageResizer.Api.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ImageController(IImageService imageService) : ImageResizerBaseController
    {
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UploadImageResponse))]
        public async Task<IActionResult> UploadAsync([FromForm] UploadFileRequest request)
        {
            return Ok(await imageService.UploadAsync(CurrentUserService.UserId, request.File));
        }
    }
}
