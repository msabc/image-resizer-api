using ImageResizer.Api.Controllers.Base;
using ImageResizer.Application.Services.Image;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImageResizer.Api.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ImageController(IImageService imageService) : ImageResizerBaseController
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> AddImageAsync([FromForm] IFormFile file)
        {
            await imageService.UploadAsync(file);

            return NoContent();
        }
    }
}
