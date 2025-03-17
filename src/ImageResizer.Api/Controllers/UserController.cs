using ImageResizer.Api.Controllers.Base;
using ImageResizer.Application.Models.Request.Auth;
using ImageResizer.Application.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace ImageResizer.Api.Controllers
{
    [Route("[controller]")]
    public class UserController(IUserService userService) : ImageResizerBaseController
    {
        [HttpPost("sign-up")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SignUpAsync([FromBody] SignUpRequest request)
        {
            return Ok(await userService.SignUpAsync(request));
        }

        [HttpPost("sign-in")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SignInAsync([FromBody] SignInRequest request)
        {
            return Ok(await userService.SignInAsync(request));
        }
    }
}
