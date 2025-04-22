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
        [HttpGet("filter")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FilterImagesReponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FilterAsync([FromBody] FilterImagesRequest request)
        {
            return Ok(await imageService.FilterAsync(CurrentUserService.UserId, request));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetByIdResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            return Ok(await imageService.GetByIdAsync(CurrentUserService.UserId, id));
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UploadImageResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadAsync([FromForm] UploadFileRequest request)
        {
            return Ok(await imageService.UploadAsync(CurrentUserService.UserId, request.File));
        }

        [HttpPatch("resize")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResizeResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResizeAsync([FromBody] ResizeRequest request)
        {
            return Ok(await imageService.ResizeAsync(CurrentUserService.UserId, request));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UploadImageResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await imageService.DeleteAsync(CurrentUserService.UserId, id);

            return Ok();
        }

        [HttpGet("generate-sas-url")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenerateSasUrlResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GenerateSasUrlAsync([FromQuery] Guid id)
        {
            return Ok(await imageService.GenerateSasUrlAsync(CurrentUserService.UserId, id));
        }
    }
}
