using ImageResizer.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImageResizer.Api.Controllers.Base
{
    [ApiController]
    public class ImageResizerBaseController : ControllerBase
    {
        private ICurrentUserService _currentUserService;

        protected ICurrentUserService CurrentUserService => _currentUserService ??= HttpContext.RequestServices.GetRequiredService<ICurrentUserService>();
    }
}
