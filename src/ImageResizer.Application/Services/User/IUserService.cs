using ImageResizer.Application.Models.Request.Auth;

namespace ImageResizer.Application.Services.User
{
    public interface IUserService
    {
        Task<string> SignUpAsync(SignUpRequest request);

        Task<string> SignInAsync(SignInRequest request);
    }
}
