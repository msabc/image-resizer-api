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
        public async Task<IActionResult> SignUpAsync([FromBody] SignUpRequest request)
        {
            return Ok(await userService.SignUpAsync(request));
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignInAsync([FromBody] SignInRequest request)
        {
            return Ok(await userService.SignInAsync(request));
        }
    }
}
