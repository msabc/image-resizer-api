using System.Security.Claims;
using ImageResizer.Domain.Models.Tables;
using Microsoft.IdentityModel.Tokens;

namespace ImageResizer.Application.Services.AccessToken
{
    public interface IAccessTokenService
    {
        Task<string> CreateAccessTokenAsync(ApplicationUser user);

        Task<(ClaimsPrincipal principal, SecurityToken validatedToken)> ValidateAccessTokenAsync(string token);
    }
}
