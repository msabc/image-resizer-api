using ImageResizer.Application.Models.Request.Auth;
using ImageResizer.Application.Services.AccessToken;
using ImageResizer.Domain.Models.Tables;
using Microsoft.AspNetCore.Identity;
using Solarnelle.Domain.Exceptions;

namespace ImageResizer.Application.Services.User
{
    public class UserService(
        UserManager<ApplicationUser> userManager,
        IAccessTokenService accessTokenService) : IUserService
    {

        public async Task<string> SignUpAsync(SignUpRequest request)
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                throw new CustomHttpException("User creation failed. Please try again later.");

            return await accessTokenService.CreateAccessTokenAsync(user);
        }

        public async Task<string> SignInAsync(SignInRequest request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user is null)
                throw new CustomHttpException("No user found with this e-mail.", System.Net.HttpStatusCode.BadRequest);

            bool signInSuccessful = await userManager.CheckPasswordAsync(user, request.Password);

            if (!signInSuccessful)
                throw new CustomHttpException("Invalid credentials.", System.Net.HttpStatusCode.Unauthorized);

            return await accessTokenService.CreateAccessTokenAsync(user);
        }
    }
}